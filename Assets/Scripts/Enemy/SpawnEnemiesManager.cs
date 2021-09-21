using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField] public int daysBetweenAttacks;
    public int variationBetweenAttacks;
    public List<int> NumEnemies;
    int prevSpawningDay = 0; // measure days from last spawnDay
    int numEnemiesIndex = 0;
    public void AttemptAttack(int currentDay, int lastSpawnDay) {
        if (2 * variationBetweenAttacks >= daysBetweenAttacks || NumEnemies.Count == 0) {
            Debug.LogError("Invalid enemy spawn settings.");
        }
        if (currentDay > lastSpawnDay)
            return;
        int relativeDay = currentDay - prevSpawningDay;
        bool isSpawnDay = (relativeDay % daysBetweenAttacks >= daysBetweenAttacks - variationBetweenAttacks
                        || relativeDay % daysBetweenAttacks <= variationBetweenAttacks);
        if (!isSpawnDay)
            return;
        bool isChosenDay = (Random.Range(-variationBetweenAttacks, variationBetweenAttacks+1) == 0);
        bool isLastChance = relativeDay >= daysBetweenAttacks + variationBetweenAttacks;
        if ((isChosenDay || isLastChance)) {
            prevSpawningDay = currentDay;
            StartAttack(NumEnemies[numEnemiesIndex]);
        }
    }
    public List<SpawnEnemies> spawnPoints;
    void StartAttack(int numEnemies) {
        for (int i = 0; i < spawnPoints.Count; i++)
            spawnPoints[i].StartAttack(numEnemies);
    }


}
