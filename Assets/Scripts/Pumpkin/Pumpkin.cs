using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : MonoBehaviour
{   
    public enum Tier
    {
        Small,
        Medium,
        Huge
    }

    private Tier currentTier;

    [Header("General")]
    public int accGrowthPoints;
    public int levelGrowthPoints = 0;    
    public int evilLevel = 0;
    public TaskSystem taskSystem;

    private float countdown = 1f;

    [Header("Water")]
    const int TotalWaterLevel = 100;
    private int waterLevel = 100;
    public int waterLossRate = 2;
    public float waterFactor = 0.25f;
    private bool inNeedOfWater = false;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        }
        else
        {
            countdown = 1;
            if(waterLevel - waterLossRate >= 0)
            {
                waterLevel -= waterLossRate;
                Debug.Log(waterLevel);
                if (waterLevel < TotalWaterLevel * waterFactor)
                {
                    if (!inNeedOfWater)
                    {
                        taskSystem.activateNeedWaterUI();
                    }
                    inNeedOfWater = true;
                }
            }
            
        }
    }  

    public void increaseWater(int amount)
    {
        Debug.Log("Increasing water");
        waterLevel += amount;
        if (inNeedOfWater)
        {
            taskSystem.closeNeedWaterUI();
            inNeedOfWater = false;
        }        
    }

    public void reduceEvilLevel(int amount)
    {
        Debug.Log("Purifying Pumpkin"); ;
        evilLevel -= amount;
    }

    public void increaseEvilLevel(int amount)
    {
        Debug.Log("Evilying Pumpkin"); ;
        evilLevel += amount;
    }

    public void increaseGrowthPoints(int amount)
    {
        Debug.Log("Increasing Pumpkin Growth points");
        levelGrowthPoints += amount;
    }
}
