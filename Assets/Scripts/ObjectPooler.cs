using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public Queue<GameObject> poolQueue;
    public List<Pool> pools;

    public static ObjectPooler Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        foreach(Pool pool in pools){
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++){
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolQueue = objectPool;
        }
    }

    public GameObject SpwanFromPool(Vector3 position, Quaternion rotation){
        GameObject objectToSpawn = poolQueue.Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolQueue.Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}