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

    // Start is called before the first frame update
    void Start()
    {
        ProcessDayN(1);
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
        StartCoroutine(showEndGamemDistribution());
    }

    IEnumerator showEndGamemDistribution()
    {
        EndLevelMessage.SetActive(true);
        messagingText.text = "The day has ended!";
        yield return new WaitForSeconds(3);
        messagingText.text = "Awarding the pumpkin: " + cycle.pumpkinPoints + "\nAwarding the plant: " + cycle.plantPoints;
        yield return new WaitForSeconds(3);
        currentDay += 1;
        messagingText.text = "Onto day " + currentDay*5;
        ProcessDayN(currentDay);
    }

    public void ProcessDayN(int n)
    {
        /*
        levelTimeMinutes = dayDurations[n-1];
        levelTimeMinutes *= 60;
        prev = levelTimeMinutes;
        countdown = prev;
        countdownText.text = prev.ToString();
        */

        SetPumpkin(n-1);

        GameObject[] plantGOs = GameObject.FindGameObjectsWithTag("Plant");
        foreach (GameObject go in plantGOs)
        {
            Plant p = go.GetComponent<Plant>();
            if (p != null && p.CHANGE_usedInLvl <= n)
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

        EndLevelMessage.SetActive(false);
    }

    public void SetPumpkin(int n)
    {
        Debug.Log("asldfjlk");
        Debug.Log(n);
        Debug.Log(pumpkinTasks[n].water);
        Debug.Log(pumpkinTasks[n].evil);
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
    