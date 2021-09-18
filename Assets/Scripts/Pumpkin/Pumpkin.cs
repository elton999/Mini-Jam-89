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
    public Tier currentTier;

    public int accGrowthPoints;

    public int levelGrowthPoints = 0;
    public int waterLevel = 0;
    public int evilLevel = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void increaseWater(int amount)
    {
        Debug.Log("Increasing water");
        waterLevel += amount;
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
