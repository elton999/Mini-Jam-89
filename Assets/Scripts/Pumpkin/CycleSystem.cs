using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleSystem : MonoBehaviour
{
    private Plant plant;
    private Pumpkin pumpkin;
    private string pumpkinTag = "Pumpkin";
    private string plantTag = "Plant";

    public int pointsPerLevel = 50;

    public static int MaxPlantGrowth = 60;
    public static float plantFactor = 0.4f;

    [HideInInspector]
    public int pumpkinPoints;
    [HideInInspector]
    public int plantPoints;

    // Start is called before the first frame update
    void Start()
    {
        pumpkin = GameObject.FindGameObjectWithTag(pumpkinTag).GetComponent<Pumpkin>();
        plant = GameObject.FindGameObjectWithTag(plantTag).GetComponent<Plant>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CalculateDistribution()
    {
        int pumpGrowth = (pumpkin.levelGrowthPoints==0)?1:pumpkin.levelGrowthPoints;
        int plantGrowth = (plant.levelGrowthPoints==0)?1:plant.levelGrowthPoints;

        Debug.Log(pumpGrowth);
        Debug.Log(plantGrowth);

        float ratio = calculateRatio(pumpGrowth, plantGrowth);

        Debug.Log(ratio);

        pumpkinPoints = (int)(pointsPerLevel * ratio);
        plantPoints = (int)(pointsPerLevel * (1 - ratio));
    }

    public void distributeGrowthPoints()
    {
        

        pumpkin.increaseGrowthPoints(pumpkinPoints);
        plant.increaseGrowthPoints(plantPoints);

        /*
        if (ratio < 0.25)
        {

        }
        else if (ratio >= 0.25 && ratio < 0.5)
        {

        }
        else if (ratio >= 0.5 && ratio < 0.75) 
        { 
        }
        else if (ratio >= 0.75 && ratio <= 1.0f)
        {

        }
        */
    }


    public float calculateRatio(int pmG, int plG)
    {
        return pmG / (float)plG;
    }
}
