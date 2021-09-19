using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskSystem : MonoBehaviour
{
    public GameObject containerW;
    public GameObject TaskBubble;
    public Text taskText;
    public GameObject TaskCompleteBubble;
    public Text taskCompleteText;

    public GameObject containerE;
    public GameObject TaskBubbleE;
    public Text taskTextE;
    public GameObject TaskCompleteBubbleE;
    public Text taskCompleteTextE;

    // Start is called before the first frame update
    void Start()
    {
        containerE.SetActive(false);
        containerW.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateNeedWaterUI()
    {
        containerW.SetActive(true);
        taskText.text = "NEW TASK:\nGive the pumpkin water!";
        TaskBubble.SetActive(true);
    }

    public void closeNeedWaterUI()
    {        
        StartCoroutine(showTaskComplete(1));
    }

    public void activateNeedEvilUI()
    {
        containerE.SetActive(true);

        taskTextE.text = "NEW TASK:\nGive the pumpkin evil potion!";
        TaskBubbleE.SetActive(true);
    }

    public void closeNeedEvilUI()
    {
        StartCoroutine(showTaskComplete(2));
    }

    IEnumerator showTaskComplete(int n)
    {
        if (n == 1)
        {
            TaskBubble.SetActive(false);
            taskCompleteText.text = "TASK COMPLETE!\nPumpkin watered.";
            yield return new WaitForSeconds(0.4f);
            TaskCompleteBubble.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            containerW.SetActive(false);
        }
        else if (n == 2)
        {
            TaskBubbleE.SetActive(false);
            taskCompleteTextE.text = "TASK COMPLETE!\nPumpkin has evil potion.";
            yield return new WaitForSeconds(0.4f);
            TaskCompleteBubbleE.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            containerE.SetActive(false);
        }
    }
}
