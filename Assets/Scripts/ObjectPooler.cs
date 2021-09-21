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
    Pool findPool(string tag) {
        for (int i = 0; i < pools.Count; i++) {
            if (pools[i].tag == tag)
                return pools[i];
        }
        return null;
    }

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
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolQueue = objectPool;
        }
    }

    public GameObject SpwanFromPool(Vector3 position, Quaternion rotation, Transform parent){
        GameObject objectToSpawn = poolQueue.Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.parent = parent;

        poolQueue.Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}