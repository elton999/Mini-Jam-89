using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    //public PumpkinMechanics pumpkin;
    // todo:
    // // Call mech.IncreaseUnholyMultiplier(onPumpkin) when unholy potion tool is used
    // // Delete tasks when the corresponding tool is used near them
    // // Animate the tool usage - darker soil for watering can (when watering can is used drop water along player's path), opacity fade for pruned plants and pickup vine items to dispose of, particles / light with the potions

    private enum Tool
    {
        WateringCan,
        Shears,
        EvilBag,
        HolyBag
    }

    private Tool selectedTool;

    public CanvasGroup w1;
    public CanvasGroup w2;
    public CanvasGroup w3;
    public CanvasGroup w4;

    public AudioClip[] SelectSounds;

    public AudioSource AS;

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
    //public Text bubbleText;

    [Header("Image")]
    public Sprite[] toolIcon;
    public Image currentImage;
    

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

        w1.alpha = 0.5f;
        

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
                currentImage.sprite = toolIcon[3];
                actionBubble.SetActive(true);
                holyPotionFlag = true;
            }
            if (withinRangePumpkin)
            {
        // todo: you can use tools even without a task nearby
                if (selectedTool == Tool.WateringCan/* && pumpkin.inNeedOfWater*/)
                {
                    if (!actionBubble.activeSelf)
                    {
                        //bubbleText.text = "Watering pumpkin...";
                        currentImage.sprite = toolIcon[0];
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
                        currentImage.sprite = toolIcon[2];
                        //bubbleText.text = "Applying evil potion...";
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
                        //bubbleText.text = "Pruning plant...";
                        currentImage.sprite = toolIcon[1];
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
            w1.alpha = 1f;
            w2.alpha = 0.5f;
            w3.alpha = 0.5f;
            w4.alpha = 0.5f;

            AS.PlayOneShot(SelectSounds[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTool = Tool.Shears;
            Debug.Log("Selected Shears");
            w1.alpha = 0.5f;
            w2.alpha = 1f;
            w3.alpha = 0.5f;
            w4.alpha = 0.5f;

            AS.PlayOneShot(SelectSounds[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedTool = Tool.EvilBag;
            Debug.Log("Selected Evil Potion");
            w1.alpha = 0.5f;
            w2.alpha = 0.5f;
            w3.alpha = 1f;
            w4.alpha = 0.5f;

            AS.PlayOneShot(SelectSounds[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedTool = Tool.HolyBag;
            Debug.Log("Selected Holy Potion");
            w1.alpha = 0.5f;
            w2.alpha = 0.5f;
            w3.alpha = 0.5f;
            w4.alpha = 1f;

            AS.PlayOneShot(SelectSounds[3]);
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
        if (collision.gameObject.tag == "Gravestone")
        {
            lvlManager.EndDay();
            CHANGE_hasLeftGravestone = false;
        }
    }
    /*

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gravestone")
        {
            CHANGE_hasLeftGravestone = true;
        }
    }
    */

}
