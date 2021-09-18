using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public int levelGrowthPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseGrowthPoints(int amount)
    {
        Debug.Log("Increasing Plant Growth points");
        levelGrowthPoints += amount;
    }

    public void reduceGrowthPoints(int amount)
    {
        Debug.Log("Reducing Plant Growth points");
        levelGrowthPoints -= amount;
    }
}
