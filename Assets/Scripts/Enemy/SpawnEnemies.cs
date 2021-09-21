using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [Range(0,5)] [SerializeField] float rangeSpawn;
    [SerializeField] Transform target;
    int numToSpawn; // set by StartAttack (called by SpawnEnemiesManager)
    int currentEnemiesNumber = 0;

    [SerializeField] float delayBetweenSpawn;
    [SerializeField] float variationBetweenSpawn;
    float currentTime = 0;
    float currentDelay = 0;
    void CalculateNewDelay() {
        currentDelay = Random.Range(delayBetweenSpawn - variationBetweenSpawn, delayBetweenSpawn + variationBetweenSpawn);
    }

    void Start() {
        CalculateNewDelay();
    }
    void Update()
    {
        if (currentEnemiesNumber < numToSpawn)
            setEnemy();
    }

    void setEnemy(){
        currentTime += Time.deltaTime;
        if(currentTime >= currentDelay){
            SpawnEnemy();
            currentTime = 0.0f;
            CalculateNewDelay();
        }
    }
    void SpawnEnemy() {
        var enemy = ObjectPooler.Instance.SpwanFromPool(
            transform.position + new Vector3(Random.Range(-rangeSpawn, rangeSpawn),Random.Range(-rangeSpawn, rangeSpawn),1), 
            Quaternion.identity,
            transform
        );
        enemy.GetComponent<Enemy>().target = target;
        currentEnemiesNumber++;
    }

    public void StartAttack(int numEnemies) {
        numToSpawn = numEnemies;
        currentEnemiesNumber = 0;
        currentTime = 0;
        CalculateNewDelay();
    }
}
