using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelTimeMinutes = 3;
    public bool isFinalLevel = false;
    private bool isEndGame = false;
    private float countdown = 3f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
            Debug.Log(countdown);
        }else if (countdown <= 0)
        {
            Debug.Log("gg");
        }

    }
}
    