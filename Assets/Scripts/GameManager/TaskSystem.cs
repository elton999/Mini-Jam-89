using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskSystem : MonoBehaviour
{
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
        
    }

    public void activateNeedWaterUI()
    {
        TaskBubble.SetActive(true);
    }

    public void closeNeedWaterUI()
    {
        taskText.text = "NEW TASK:\nGive the pumpkin water!";
        TaskBubble.SetActive(false);
        StartCoroutine(showTaskComplete(1));
        
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
            TaskCompleteBubble.SetActive(false);
        }        
    }
}
