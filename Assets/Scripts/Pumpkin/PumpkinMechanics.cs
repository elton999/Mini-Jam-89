using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PumpkinAnimations), typeof(TaskSpawner))]
public class PumpkinMechanics : MonoBehaviour
{

    [HideInInspector] public PumpkinAnimations anim;
    [HideInInspector] public TaskSpawner tasks;
    void Start() {
        anim = GetComponent<PumpkinAnimations>();
        tasks = GetComponent<TaskSpawner>();
        ResetGrowth();
    }


    public bool NeedsWater() {
        return false;
    }
    public bool NeedsPrune() {
        return false;
    }

    // Fertilizer (Unholy Potion Tool)
    // // affects enemy spawn counts, grow speed...
    [HideInInspector] public float unholyMultiplier = 1;
    [SerializeField] float deltaUnholyTool = 0.1f;
    [SerializeField] float deltaUnholyOnPumpkin = 0.5f;
    [SerializeField] float maxUnholyMultiplier = 3;
    public void IncreaseUnholyMultiplier(bool onPumpkin) { // called by Inventory script
        if (onPumpkin) unholyMultiplier += deltaUnholyOnPumpkin;
        else unholyMultiplier += deltaUnholyTool;
        unholyMultiplier = Mathf.Clamp(unholyMultiplier, 1, maxUnholyMultiplier);
    }

    public int IdealGrowthCycles = 20;
    public float idealPlantSize = 0.4f;
    public float plantPenaltySeverity = 5; // at 0.2f plant size deviation, pumpkin will 0.5x as fast than at ideal separation. plant growth unaffected
    public int pumpkinWaters = 0;
    public int plantWaters = 0;
    public void DoGrowth() {
        pumpkinWaters--;

        plantWaters -= 2;
        // Calculate growth using completion percentage (tasks.currentTasks / numTasks)
        float waterCompletion = tasks.wateringTasks.GetTaskCompletion();
        float pruneCompletion = tasks.pruningTasks.GetTaskCompletion();
        float repairCompletion = tasks.repairTasks.GetTaskCompletion();
        plantSize += 0;
        
        // Completing all tasks for 20 days will result in pumpkinSize=1
        // Overwatering increases plant growth, and overpruning decreases plant size directly
// todo: adjust this formula, adding ramping. currently punishes too hard for small deviations
        float plantSizePenalty = 1 + plantPenaltySeverity * Mathf.Abs(plantSize - idealPlantSize);
        float pumpkinSizeIncrease = 1f / IdealGrowthCycles;
        pumpkinSizeIncrease /= plantSizePenalty;
        pumpkinSizeIncrease *= unholyMultiplier; // unholy potion gives a size boost
        pumpkinSizeIncrease *= waterCompletion;
        pumpkinSizeIncrease *= pruneCompletion;
        pumpkinSizeIncrease *= repairCompletion;
        
        unholyMultiplier = 1; // reset for next day
        // if the player uses a tool around the vine area, and it doesnt fulfill a task, add a spike to plant growth
        pumpkinSize += pumpkinSizeIncrease;
    }

    

    [Range(0,1)] public float pumpkinSize;
    [Range(0,1)] public float plantSize;
    void ResetGrowth() {
        pumpkinSize = 0;
        plantSize = 0;
        anim.UpdateVisualPumpkin(pumpkinSize, plantSize);
    }
}
