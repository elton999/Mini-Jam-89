using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSpawner : MonoBehaviour
{

    public GameObject waterTaskBubble; // water, on pumpkin AND vines, most days (when water is low)
    public GameObject pruneTaskBubble; // prune, on outer vines, all days except combat (when plant vines are too big, then size reduces next day)
    public GameObject unholyTaskBubble; // fertilize, on pumpkin, 1 day before combat
    public GameObject repairTaskBubble; // repair the damage dealt by the ghosts, on vines OR pumpkin, 1-2 days after combat. can also be solved by prune tool!

    

    // // Pumpkin anims are only updated between days
    // Tools can be used regardless of tasks. But when used near a task, it deletes that task for 
    // // This enables overwatering / overpruning (less growth), and overfertilizing (more combat days)
    // Pumpkin spawns more tasks as it gets bigger
    // Tasks remain across days - never destroyed unless completed.

    // Enemies always move directly toward pumpkin.
    // Enemies damage any vines they collide with - dont destroy
    // // Sometimes they'll stop to suck at a vine or the pumpkin, which reduces pumpkin size.
    // // After a suck is finished (time-based), a repair bubble is immediately spawned.
    // Damaged vines are included in the pumpkin growth limiter, but not included in the pumpkin growth enhancer

    // Note: call spm.AttemptAttack before spawning tasks
    public PumpkinAnimations pumpkin;
    public SpawnEnemiesManager spm;
    public List<float> repairProportions; // 0.5f at index 0 means: spawn 50% of the repair tasks 1 day after combat
    public void SpawnTasks(int currentDay) {
        int daysSinceAttack = spm.GetDaysSinceAttack(currentDay);
        bool needWater = false;
        bool needPrune = false;
        // Basic plant care
        if (pumpkin.mech.NeedsWater()) {
            SpawnWateringTasks();
        }
        if (pumpkin.mech.NeedsPrune() && daysSinceAttack > 0) {
            SpawnPruningTasks();
        }
        // Fertilize
        if (currentUnholyTasks.Count == 0 && daysSinceAttack >= spm.daysBetweenAttacks-1) {
            SpawnUnholyTasks();
        }
        if (daysSinceAttack == 0) {
            for (int i = 0; i < currentUnholyTasks.Count; i++) {
                Destroy(currentUnholyTasks[i]);
                currentUnholyTasks.RemoveAt(i);
            }
        }
        // Repair
        if (enemyDamagePoints.Count > 0 && daysSinceAttack > 0) {
            float proportion = repairProportions[daysSinceAttack-1];
            SpawnRepairTasks(proportion);
        }
    }
    public List<Vector2> enemyDamagePoints; // filled by Enemy script
    void SpawnRepairTasks(float proportion) {
        proportion = Mathf.Clamp(proportion, 0, 1);
        int numCurrentRepairs = Mathf.RoundToInt(proportion * enemyDamagePoints.Count);
        List<Vector2> currentRepairPoints = new List<Vector2>();
        // Decide which damagePoints to repair
        for (int i = 0; i < numCurrentRepairs; i++) {
            int index = Random.Range(0, enemyDamagePoints.Count);
            currentRepairPoints.Add(enemyDamagePoints[index]);
            enemyDamagePoints.RemoveAt(index);
        }
        // Spawn the tasks
        for (int i = 0; i < currentRepairPoints.Count; i++) {
            currentRepairTasks.Add(SpawnTaskAt(currentRepairPoints[i], repairTaskBubble));
        }
    }
    void SpawnWateringTasks() {

    }
    void SpawnPruningTasks() {

    }
    void SpawnUnholyTasks() {

    }

    public List<GameObject> currentRepairTasks; // gets cleared by Player
    public List<GameObject> currentWateringTasks;
    public List<GameObject> currentPruningTasks;
    public List<GameObject> currentUnholyTasks;
    
    GameObject SpawnTaskAt(Vector2 position, GameObject prefab) {
        GameObject task = Instantiate(prefab, transform);
        task.transform.position = position;
        //task.transform.Find("Sprite").GetComponent<SpriteAnimations>().StartAnimation();
        return task;
    }
    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
