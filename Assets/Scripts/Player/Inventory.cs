using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private enum Tool
    {
        WateringCan,
        Shears,
        EvilBag,
        HolyBag
    }

    private Tool selectedTool;

    //will only work with one pumpkin
    [Header("Objects")]
    private Pumpkin pumpkin;
    private Plant plant;

    private bool withinRangePumpkin = false;
    private bool withinRangePlant = false;
    private string pumpkinTag = "Pumpkin";
    private string plantTag = "Plant";

    [Header("Bubble")]
    public GameObject actionBubble;
    public Text bubbleText;

    [Header("Tools and Amounts")]
    public int waterAmount = 100;
    public int shearAmount = 5;
    public int evilAmount = 5;
    public int purifyrAmount = 5;
    public int growthAmount = 5;


    // Start is called before the first frame update
    void Start()
    {
        selectedTool = Tool.WateringCan;
        pumpkin = GameObject.FindGameObjectWithTag(pumpkinTag).GetComponent<Pumpkin>();
        plant = GameObject.FindGameObjectWithTag(plantTag).GetComponent<Plant>();
        actionBubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        updateSelectedTool();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (withinRangePumpkin)
            {
                if (selectedTool == Tool.WateringCan)
                {
                    StartCoroutine(waterPlantAction());
                }
                else if (selectedTool == Tool.EvilBag)
                {
                    pumpkin.increaseEvilLevel(evilAmount);
                }
                else if (selectedTool == Tool.HolyBag)
                {
                    pumpkin.reduceEvilLevel(purifyrAmount);
                }
            }
            else if(withinRangePlant)
            {
                if (selectedTool == Tool.Shears)
                {
                    plant.reduceGrowthPoints(shearAmount);
                }
            }            
        }
        else
        {
            if(withinRangePumpkin && (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A)||
                Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D)))
            {
                actionBubble.SetActive(false);
            }            
        }
    }
    //Tool selection
    private void updateSelectedTool()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTool = Tool.WateringCan;
            Debug.Log("Selected 1");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTool = Tool.Shears;
            Debug.Log("Selected 2");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedTool = Tool.EvilBag;
            Debug.Log("Selected 3");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedTool = Tool.HolyBag;
            Debug.Log("Selected 4");
        }
    }

    //Tool Actions    

    IEnumerator waterPlantAction()
    {
        bubbleText.text = "Watering pumpkin...";
        actionBubble.SetActive(true);
        yield return new WaitForSeconds(3);
        if(actionBubble.activeSelf)
            pumpkin.increaseWater(waterAmount);
    }

    //Collisions

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == pumpkinTag)
        {
            withinRangePumpkin = true;
        }
        else if(collision.gameObject.tag == plantTag)
        {
            withinRangePlant = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == pumpkinTag)
        {
            withinRangePumpkin = false;
        }
        else if (collision.gameObject.tag == plantTag)
        {
            withinRangePlant = false;
        }
    }

}
