using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleSystem : MonoBehaviour
{



    [Header("Level Details")]
    [SerializeField] int currentDay = 1;
    [SerializeField] SpawnEnemiesManager spm;
    [SerializeField] PumpkinMechanics pumpkin;
    [SerializeField] CanvasController ui;
    [SerializeField] TaskSpawner tasks;
    [SerializeField] int maxSpawnDay;
    public void NextDay() { // To go to next day, player just enters the gravehouse
        // If tasks are left incomplete, popup Are you sure?
        // fade to black, lower / slow music

        // advance to next day
        int daysSinceAttack = spm.GetDaysSinceAttack(currentDay); // gets reset in AttemptAttack(), but required for SpawnTasks()
        float unholyMultiplier = pumpkin.unholyMultiplier; // gets reset in DoGrowth(), but required for AttemptAttack()
        float oldPumpkinProgress = pumpkin.pumpkinSize; // gets changed in DoGrowth(), but required for UI slider anim
        pumpkin.DoGrowth();
        pumpkin.anim.UpdateVisualPumpkin(pumpkin.pumpkinSize, pumpkin.plantSize);
        pumpkin.tasks.SpawnTasks(daysSinceAttack);

        // display task completion summary
        //ui.SetSize(pumpkin.pumpkinProgress, oldPumpkinProgress);
        // wait until player hits ok (or countdown?)
        
        // start day
        ui.SetDay(currentDay); // update UI
        spm.AttemptAttack(currentDay, maxSpawnDay, unholyMultiplier);
        // fade from black
        // bell sfx
        // resume music
        // show UI alerts - UI alerts are whispers from the pumpkin
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
            NextDay();
// todo: if the player still has remaining tasks, popup Are you sure?
    }

}
