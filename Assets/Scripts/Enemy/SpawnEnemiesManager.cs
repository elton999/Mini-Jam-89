using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesManager : MonoBehaviour
{

    public int daysBetweenAttacks; // "center" of each spawn window - ex: center=4, variation=1 means first attack can occur between days 3-5. next attack window has a center 4 days after the prev attack (so first day 3, second day 6 is possible)
    public int variationBetweenAttacks; // "width" of the spawn window
    public List<int> NumEnemies; // ex: 2 at index 0 means each spawnPoint will create at least 2 enemies on the first attack
    int numEnemiesIndex = 0; // ++ after each attack
    int prevSpawningDay = 0; // measure days from last spawnDay
    public void AttemptAttack(int currentDay, int lastSpawnDay, float unholyMultiplier) {
        if (/*2 * variationBetweenAttacks >= daysBetweenAttacks || */NumEnemies.Count == 0) {
            Debug.LogError("Invalid enemy spawn settings.");
            // if variation span is bigger than the center, attacks might occur on consecutive days
        }
        if (currentDay > lastSpawnDay) {
            return;
        }
        int relativeDay = currentDay - prevSpawningDay;
        bool inWindow = (relativeDay % daysBetweenAttacks >= daysBetweenAttacks - variationBetweenAttacks
                        || relativeDay % daysBetweenAttacks <= variationBetweenAttacks);
        if (!inWindow) {
            return;
        }
        bool isChosenDay = (Random.Range(-variationBetweenAttacks, variationBetweenAttacks+1) == 0);
        bool isLastChance = relativeDay >= daysBetweenAttacks + variationBetweenAttacks;
        if ((isChosenDay || isLastChance)) {
            prevSpawningDay = currentDay;
            numEnemiesIndex++;
            StartAttack(NumEnemies[numEnemiesIndex], unholyMultiplier);
        }
    }
    [SerializeField] List<SpawnEnemies> spawnPoints;
    void StartAttack(int numEnemies, float unholyMultiplier) {
        for (int i = 0; i < spawnPoints.Count; i++)
            spawnPoints[i].StartAttack(numEnemies, unholyMultiplier);
    }
    public int GetDaysSinceAttack(int currentDay) {
        return currentDay - prevSpawningDay;
    }


}
