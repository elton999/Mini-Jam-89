using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleSystem : MonoBehaviour
{
    private List<Plant> plants;
    [HideInInspector]
    public Pumpkin pumpkin;
    private string pumpkinTag = "Pumpkin";
    private string plantTag = "Plant";

    [HideInInspector]
    public int CHANGE_plantsToBePruned = 0;
    public float CHANGE_plantPercentage = 0.75f;

    public int pointsPerLevel = 50;

    public int growthPointsPerTaskDone = 50;
    public int pointsPerPlantUnPruned = 40;

    public static int MaxPlantGrowth = 1000;
    public static float plantFactor = 0.6f;

    [HideInInspector]
    public int pumpkinPoints;
    [HideInInspector]
    public int totalPoints;
    [HideInInspector]
    public int plantPoints;

    // Start is called before the first frame update
    void Start()
    {
        pumpkin = GameObject.FindGameObjectWithTag(pumpkinTag).GetComponent<Pumpkin>();
        plants = new List<Plant>();
        GameObject [] plantGOs = GameObject.FindGameObjectsWithTag(plantTag);
        foreach(GameObject go in plantGOs)
        {
            plants.Add(go.GetComponent<Plant>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CalculateDistribution()
    {
        int totalPlantGrowthPoints = 0;
        foreach(Plant p in plants)
        {
            if (p.inNeedOfPruning)
            {
                totalPlantGrowthPoints += 1;
            }
        }
        
        Debug.Log(totalPlantGrowthPoints);

        float ratio = calculateRatio(totalPlantGrowthPoints);

        Debug.Log(ratio);
        plantPoints = (int)(pointsPerLevel * ratio * CHANGE_plantPercentage);
        pumpkinPoints = (int)(pointsPerLevel*(1-CHANGE_plantPercentage) - (int)((((pumpkin.inNeedOfWater) ? 1 : 0) + ((pumpkin.inNeedOfEvil) ? 1 : 0) / 2) * (1 - CHANGE_plantPercentage)*pointsPerLevel))
            + (int)(pointsPerLevel * (1- ratio) * CHANGE_plantPercentage);
        totalPoints += pumpkinPoints;


    }

    public void distributeGrowthPoints()
    {
        pumpkin.increaseGrowthPoints(pumpkinPoints);
    }


    public float calculateRatio(int plG)
    {
        if(CHANGE_plantsToBePruned == 0)
        {
            return 1;
        }
        return plantFactor = ((float)plG / CHANGE_plantsToBePruned);
    }
}
