using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleSystem : MonoBehaviour
{
    private List<Plant> plants;
    private Pumpkin pumpkin;
    private string pumpkinTag = "Pumpkin";
    private string plantTag = "Plant";

    public int pointsPerLevel = 50;

    public int growthPointsPerTaskDone = 50;
    public int pointsPerPlantUnPruned = 40;

    public static int MaxPlantGrowth = 1000;
    public static float plantFactor = 0.6f;

    [HideInInspector]
    public int pumpkinPoints;
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
        int pumpGrowth = (pumpkin.waterTasks - pumpkin.currWaterTasks) * growthPointsPerTaskDone +
            (pumpkin.evilTasks - pumpkin.currEvilTasks) * growthPointsPerTaskDone;

        int totalPlantGrowthPoints = 0;
        foreach(Plant p in plants)
        {
            if (p.inNeedOfPruning)
            {
                totalPlantGrowthPoints += pointsPerPlantUnPruned;
            }
        }

        Debug.Log(pumpGrowth);
        Debug.Log(totalPlantGrowthPoints);

        float ratio = calculateRatio(pumpGrowth, totalPlantGrowthPoints);

        Debug.Log(ratio);

        pumpkinPoints = (int)(pointsPerLevel * ratio);
        plantPoints = (int)(pointsPerLevel * (1 - ratio));
    }

    public void distributeGrowthPoints()
    {
        

        pumpkin.increaseGrowthPoints(pumpkinPoints);

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
        return pmG / ((float)plG + pmG);
    }
}
