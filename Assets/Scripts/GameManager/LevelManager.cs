using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Level Manager Details")]
    public int currentDay = 1;
    public int[] dayDurations;

    
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
        }        
    }

    IEnumerator showEndGamemDistribution()
    {
        EndLevelMessage.SetActive(true);
        messagingText.text = "The day has ended!";        
        yield return new WaitForSeconds(3);
        messagingText.text = "Awarding the pumpkin: " + cycle.pumpkinPoints +"\nAwarding the plant: " + cycle.plantPoints ;
        yield return new WaitForSeconds(3);
        currentDay += 5;
        messagingText.text = "Onto day " + currentDay;
    }

    public void ProcessDayN(int n)
    {
        levelTimeMinutes = dayDurations[n-1];
        levelTimeMinutes *= 60;
        prev = levelTimeMinutes;
        countdown = prev;
        countdownText.text = prev.ToString();
        EndLevelMessage.SetActive(false);
    }
}
    