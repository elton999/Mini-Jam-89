using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int currentDay = 1;
    public int levelTimeMinutes = 3;
    public bool isFinalLevel = false;
    private bool isEndGame = false;
    private float countdown = 4f;
    
    private int prev;
    public Text countdownText;

    public Text messagingText;

    public CycleSystem cycle;


        



    // Start is called before the first frame update
    void Start()
    {
        levelTimeMinutes *= 1;
        prev = levelTimeMinutes;
        countdown = prev;
        countdownText.text = prev.ToString();
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

        messagingText.text = "The day has ended!";        
        yield return new WaitForSeconds(3);
        messagingText.text = "Awarding the pumpkin: " + cycle.pumpkinPoints +"\nAwarding the plant: " + cycle.plantPoints ;
        yield return new WaitForSeconds(3);
        currentDay += 5;
        messagingText.text = "Onto day " + currentDay;
    }
}
    