using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
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
            */
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
        messagingText.text = "The day has ended!\nAwarding the pumpkin: " + cycle.pumpkinPoints + "\nAwarding the plant: " + cycle.plantPoints + "\nOnto day " + currentDay * 5;        
    }

    public void ProcessDayN()
    {        
        EndLevelMessage.SetActive(false);
        /*
        levelTimeMinutes = dayDurations[n-1];
        levelTimeMinutes *= 60;
        prev = levelTimeMinutes;
        countdown = prev;
        countdownText.text = prev.ToString();
        */

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
        if (pumpkinTasks[n].water)
        {
            cycle.pumpkin.ActivateWaterTask();
        }

        if (pumpkinTasks[n].evil)
        {
            cycle.pumpkin.ActivateEvilTask();
        }

    }
}
    