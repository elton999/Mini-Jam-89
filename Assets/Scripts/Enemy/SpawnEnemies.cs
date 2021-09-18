using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [Range(0,5)] [SerializeField] float rangeSpawn;
    [SerializeField] int maxSpawn;
    int currentEnemiesNumber = 0;
    [SerializeField] float delayBetweenSpawn;
    float currentTime = 0;
    [SerializeField] GameObject enemyObject;
    [SerializeField] Transform target;

    void Update()
    {
        if(maxSpawn <= currentEnemiesNumber)
            return;

        setEnemy();
    }

    void setEnemy(){
        currentTime += Time.deltaTime;
        if(currentTime >= delayBetweenSpawn){
            var enemy = Instantiate(
                enemyObject, 
                transform.position + new Vector3(Random.Range(-rangeSpawn, rangeSpawn),Random.Range(-rangeSpawn, rangeSpawn),1), 
                Quaternion.identity
            );
            enemy.GetComponent<Enemy>().target = target;
            currentTime = 0.0f;
            currentEnemiesNumber++;
        }
    }
}
