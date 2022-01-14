using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    // This code isnt used - it's been adapted into PumpkinMechanics and CycleSystem

    /*
    [Header("Level Manager Details")]
    public int currentDay = 1;
    public TaskSerializable[] pumpkinTasks;


    [Header("Level Details")]
    public int levelTimeMinutes = 3;
    public bool isFinalLevel = false;
    private bool isEndGame = false;
    private float countdown = 4f;

    private int prev;
    public Text countdownText;

    public GameObject EndLevelMessage;
    public Text messagingText;

    public CycleSystem cycle;

    public UIAnimations uiAnim;
    public GameObject unimGO;

    public SpawnEnemiesManager spm;
    public int maxSpawnDay; // move this to a diff script probably

    // Start is called before the first frame update
    void Start()
    {
        ProcessDayN();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEndGame)
        {
            /*
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
                countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
                if ((int)countdown != prev)
                {
                    prev = (int)countdown;
                    countdownText.text = prev.ToString();
                }

            }
            else if (countdown <= 0)
            {
                isEndGame = true;
                cycle.CalculateDistribution();
                StartCoroutine(showEndGamemDistribution());

            }
            *//*
        }
    }

    public void EndDay()
    {
        cycle.CalculateDistribution();
        unimGO.SetActive(true);
        uiAnim.StartAnimation();
        StartCoroutine(showEndGamemDistribution());
    }

    IEnumerator showEndGamemDistribution()
    {
        
        yield return new WaitForSeconds(1);
        EndLevelMessage.SetActive(true);
        messagingText.text = "The day has ended!\nAwarding the pumpkin: " + cycle.pumpkinPoints + "\nAwarding the plant: " + cycle.plantPoints + "\nTotal Points: " + cycle.totalPoints * 5;        
    }

    public Text gameText;
    public GameObject endGO;

// todo: move this to CycleSystem
    public void ProcessDayN()
    {
        if (currentDay -1 == pumpkinTasks.Length) 
        {
            gameText.text = "Final points: " + cycle.totalPoints;
            endGO.SetActive(true);
            return;
            
        }

        spm.AttemptAttack(currentDay, maxSpawnDay);
        Camera.main.GetComponent<CameraController>().cc.SetDay(currentDay);

        EndLevelMessage.SetActive(false);
        /*
        levelTimeMinutes = dayDurations[n-1];
        levelTimeMinutes *= 60;
        prev = levelTimeMinutes;
        countdown = prev;
        countdownText.text = prev.ToString();
        *//*

        SetPumpkin(currentDay-1);

        GameObject[] plantGOs = GameObject.FindGameObjectsWithTag("Plant");
        foreach (GameObject go in plantGOs)
        {
            Plant p = go.GetComponent<Plant>();
            if (p != null && p.CHANGE_usedInLvl <= currentDay)
            {
                go.SetActive(true);
                int x = Random.Range(1, 3);
                Debug.Log(x);
                if (x == 1)
                {
                    p.activatePruneUI();
                    cycle.CHANGE_plantsToBePruned++;
                }
            }
            else
            {
                go.SetActive(false);
            }
        }
        currentDay++;
        EndLevelMessage.SetActive(false);
    }

    public void SetPumpkin(int n)
    {
        cycle.pumpkin.enemysInPumpkin = 0;
        if (pumpkinTasks[n].water)
        {
            cycle.pumpkin.ActivateWaterTask();
        }

        if (pumpkinTasks[n].evil)
        {
            cycle.pumpkin.ActivateEvilTask();
        }

    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Main - ending");
    }


// moved this stuff from CycleSystem
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
        GameObject[] plantGOs = GameObject.FindGameObjectsWithTag(plantTag);
        totalPoints = 0;
        foreach(GameObject go in plantGOs)
        {
            plants.Add(go.GetComponent<Plant>());
        }
        pa = transform.parent.GetComponent<PumpkinAnimations>();
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
            + (int)(pointsPerLevel * (1- ratio) * CHANGE_plantPercentage) - pumpkin.enemysInPumpkin;
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
    */
}
    