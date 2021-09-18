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

    public void distributeGrowthPoints()
    {
        int pumpGrowth = pumpkin.levelGrowthPoints;
        int plantGrowth = plant.levelGrowthPoints;

        float ratio = calculateRatio(pumpGrowth, plantGrowth);

        pumpkin.increaseGrowthPoints( (int)(pointsPerLevel * ratio));
        plant.increaseGrowthPoints((int)(pointsPerLevel * (1/ratio)));

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
        return pmG / plG;
    }
}
