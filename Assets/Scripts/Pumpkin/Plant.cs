using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour
{
    [HideInInspector]
    public int levelGrowthPoints = 0;
    
    private float countdown=1f;

    [HideInInspector]
    public bool inNeedOfPruning = false;

    public GameObject TaskBubble;
    public Text taskText;
    public GameObject TaskCompleteBubble;
    public Text taskCompleteText;

    // Start is called before the first frame update
    void Start()
    {
        TaskBubble.SetActive(false);
        TaskCompleteBubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown < 0)
        {
            countdown = 1f;
            if (levelGrowthPoints <= CycleSystem.MaxPlantGrowth)
            {
                increaseGrowthPoints(4);
                if(levelGrowthPoints> CycleSystem.MaxPlantGrowth * CycleSystem.plantFactor)
                {
                    inNeedOfPruning = true;
                    activateNeedWaterUI();
                }
            }
        }
        
    }

    public void increaseGrowthPoints(int amount)
    {        
        levelGrowthPoints += amount;
        Debug.Log(levelGrowthPoints);
    }

    public void reduceGrowthPoints(int amount)
    {
        Debug.Log("Reducing Plant Growth points");
        levelGrowthPoints -= amount;
        inNeedOfPruning = false;
        closeNeedWaterUI();
    }

    public void activateNeedWaterUI()
    {
        taskText.text = "NEW TASK:\nPrune the plants!";
        TaskBubble.SetActive(true);
    }

    public void closeNeedWaterUI()
    {        
        StartCoroutine(showTaskComplete(1));
    }

    IEnumerator showTaskComplete(int n)
    {
        if (n == 1)
        {
            TaskBubble.SetActive(false);
            taskCompleteText.text = "TASK COMPLETE!\nPlant is pruned.";
            yield return new WaitForSeconds(0.4f);
            TaskCompleteBubble.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            TaskCompleteBubble.SetActive(false);
        }
    }
}
