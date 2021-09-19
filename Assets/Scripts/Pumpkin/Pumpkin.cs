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
    private int accGrowthPoints;
    [HideInInspector]
    public int levelGrowthPoints = 0;    
    
    public TaskSystem taskSystem;
    private bool taskEnabled = false;

    //initial delay
    public int initialDelay = 3;
    private int initialCountdown = 3;
    private bool initialHasEnded = false;

    //Time between tasks
    public int timeBetweenTasks = 3;
    private int tbtCountdown = 3;
    
    public int waterTasks = 1;
    [HideInInspector]
    public int currWaterTasks;
    public int evilTasks = 1;
    [HideInInspector]
    public int currEvilTasks;

    private float countdown = 1f;

    [Header("Water")]
    //public bool CHANGE_waterTask;
    const int TotalWaterLevel = 100;
    private int waterLevel = 100;
    public int waterLossRate = 2;
    public float waterFactor = 0.25f;
    [HideInInspector]
    public bool inNeedOfWater = false;
    private bool isWaterTask = false;
    
    [Header("Evil Potion")]
    //public bool CHANGE_evilTask;
    public int evilWaitTime = 5;
    [HideInInspector]
    public bool inNeedOfEvil = false;
    private bool isEvilTask = true;


    // Start is called before the first frame update
    void Start()
    {
        /*
        initialCountdown = initialDelay;
        currWaterTasks = waterTasks;
        currEvilTasks = evilTasks;
        tbtCountdown = timeBetweenTasks;
        */
    }

    public void ActivateWaterTask()
    {
        inNeedOfWater = true;
        taskSystem.activateNeedWaterUI();
    }

    public void ActivateEvilTask()
    {
        inNeedOfEvil = true;
        taskSystem.activateNeedEvilUI();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        }
        else
        {
            countdown = 1;

            if (!initialHasEnded)
            {
                initialCountdown--;
                if (initialCountdown <= 0)
                {
                    initialCountdown = 0;
                    initialHasEnded = true;
                    DecideTasks();
                }
            }
            else
            {
                if (taskEnabled)
                {
                    ManageTasks();
                }
                else
                {
                    Debug.Log(tbtCountdown);
                    tbtCountdown--;
                    if (tbtCountdown <= 0)
                    {
                        tbtCountdown = timeBetweenTasks;
                        DecideTasks();
                    }
                }
            }
        } 
        */
    }

    public void DecideTasks()
    {
        Debug.Log("Deciding Tasks");
        if(currEvilTasks>0 || currWaterTasks > 0)
        {
            int randNum = Random.Range(1,3);
            Debug.Log("RandNum is " + randNum);
            if (randNum == 1)
            {
                if (currWaterTasks == 0)
                {
                    ToggleEvilTask();
                }
                else
                {
                    ToggleWaterTask();
                }
            }
            else
            {
                if (currEvilTasks == 0)
                {
                    ToggleWaterTask();
                }
                else
                {
                    ToggleEvilTask();
                }

            }
            EnableTasks();
        }
        
    }
    
    public void ManageTasks()
    {
        if (isWaterTask)
        {
            if (waterLevel - waterLossRate >= 0)
            {
                waterLevel -= waterLossRate;
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
        else if (isEvilTask)
        {
            if (evilWaitTime <= 0)
            {                
                if (!inNeedOfEvil)
                {
                    taskSystem.activateNeedEvilUI();
                    inNeedOfEvil = true;
                    evilWaitTime = 5;
                }
            }
            else
            {
                if (!inNeedOfEvil)
                {
                    evilWaitTime--;
                }
            }
        }
    }

    //Task balance
    public void ToggleWaterTask()
    {
        Debug.Log("Toggling Water Task");
        isWaterTask = true;
        isEvilTask = false;
    }

    public void ToggleEvilTask()
    {
        Debug.Log("Toggling Evil Task");
        isWaterTask = false;
        isEvilTask = true;
    }

    public void EnableTasks()
    {
        taskEnabled = true;
    }

    public void DisableTasks()
    {
        taskEnabled = false;
        isWaterTask = false; 
        isEvilTask = false;
    }

    //Actions

    public void increaseWater(int amount)
    {
        Debug.Log("Increasing water");
        waterLevel += amount;
        currWaterTasks--;
        if (inNeedOfWater)
        {
            taskSystem.closeNeedWaterUI();
            inNeedOfWater = false;
        }
        DisableTasks();
    }


    public void increaseEvilLevel()
    {
        Debug.Log("Evilying Pumpkin");
        currEvilTasks--;
        if (inNeedOfEvil)
        {
            taskSystem.closeNeedEvilUI();
            inNeedOfEvil = false;
        }
        DisableTasks();
    }

    public void increaseGrowthPoints(int amount)
    {
        Debug.Log("Increasing Pumpkin Growth points");
        levelGrowthPoints += amount;
    }
}
