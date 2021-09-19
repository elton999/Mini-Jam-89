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

    [HideInInspector]
    public bool holyPotionFlag = false;

    public bool CHANGE_hasLeftGravestone = false;

    //will only work with one pumpkin
    [Header("Objects")]
    private Pumpkin pumpkin;
    private Plant plant = null;
    private LevelManager lvlManager;

    private bool withinRangePumpkin = false;
    private bool withinRangePlant = false;
    private string pumpkinTag = "Pumpkin";
    private string plantTag = "Plant";

    [Header("Bubble")]
    public GameObject actionBubble;
    public Text bubbleText;

    [Header("Tools and Amounts")]
    public int waterAmount = 100;

    public float waterTaskTime = 3f;
    public float plantTaskTime = 3f;
    public float evilTaskTime = 3f;

    private float waterCountdown = 3f;
    private float plantCountdown = 3f;
    private float evilCountdown = 3f;


    // Start is called before the first frame update
    void Start()
    {
        selectedTool = Tool.WateringCan;
        pumpkin = GameObject.FindGameObjectWithTag(pumpkinTag).GetComponent<Pumpkin>();
        lvlManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        plant = null;
        actionBubble.SetActive(false);
        waterTaskTime = waterCountdown;
        plantTaskTime = plantCountdown;
        evilTaskTime = evilCountdown;
    }


    // Update is called once per frame
    void Update()
    {
        updateSelectedTool();
        if (Input.GetKey(KeyCode.E))
        {
            if (selectedTool == Tool.HolyBag)
            {                
                holyPotionFlag = true;
            }
            if (withinRangePumpkin)
            {
                if (selectedTool == Tool.WateringCan && pumpkin.inNeedOfWater)
                {
                    if (!actionBubble.activeSelf)
                    {
                        bubbleText.text = "Watering pumpkin...";
                        actionBubble.SetActive(true);
                    }                    
                    waterCountdown -= Time.deltaTime;
                    if (waterCountdown <= 0)
                    {
                        resetWaterAction();
                        pumpkin.increaseWater(waterAmount);                        
                    }
                }
                else if (selectedTool == Tool.EvilBag && pumpkin.inNeedOfEvil)
                {
                    if (!actionBubble.activeSelf)
                    {
                        bubbleText.text = "Applying evil potion...";
                        actionBubble.SetActive(true);
                    }
                    evilCountdown -= Time.deltaTime;
                    if (evilCountdown <= 0)
                    {
                        resetEvilAction();
                        pumpkin.increaseEvilLevel();
                    }                    
                }
            }
            if (withinRangePlant)
            {
                if(selectedTool == Tool.Shears && plant!=null && plant.inNeedOfPruning)
                {
                    if (!actionBubble.activeSelf)
                    {
                        bubbleText.text = "Pruning plant...";
                        actionBubble.SetActive(true);
                    }
                    plantCountdown -= Time.deltaTime;
                    if (plantCountdown <= 0)
                    {
                        plant.reduceGrowthPoints();
                        resetPlantAction();                        
                    }                    
                }
            }
        } else if (Input.GetKeyUp(KeyCode.E))
        {
            if (selectedTool == Tool.HolyBag)
            {                
                holyPotionFlag = false;                
            }
            resetWaterAction();
            resetPlantAction();
        }
    }

    private void resetEvilAction()
    {
        actionBubble.SetActive(false);
        evilCountdown = evilTaskTime;
    }

    private void resetPlantAction()
    {
        actionBubble.SetActive(false);
        plantCountdown = plantTaskTime;
        plant = null;

    }

    private void resetWaterAction()
    {
        actionBubble.SetActive(false);
        waterCountdown = waterTaskTime;

    }

    //Tool selection
    private void updateSelectedTool()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTool = Tool.WateringCan;
            Debug.Log("Selected Watering Can");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTool = Tool.Shears;
            Debug.Log("Selected Shears");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedTool = Tool.EvilBag;
            Debug.Log("Selected Evil Potion");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedTool = Tool.HolyBag;
            Debug.Log("Selected Holy Potion");
        }
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
            plant = collision.gameObject.GetComponent<Plant>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == pumpkinTag)
        {
            withinRangePumpkin = false;
            resetWaterAction();
        }
        else if (collision.gameObject.tag == plantTag)
        {
            withinRangePlant = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gravestone" && CHANGE_hasLeftGravestone)
        {
            lvlManager.EndDay();
            CHANGE_hasLeftGravestone = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gravestone")
        {
            CHANGE_hasLeftGravestone = true;
        }
    }

}
