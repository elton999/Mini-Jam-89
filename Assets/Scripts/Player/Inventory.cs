using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Tools")]
    public int waterAmount = 5;
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
                    pumpkin.increaseWater(waterAmount);
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
    }

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

    public void withinPumpkinContact(Pumpkin p)
    {
        pumpkin = p;
    }

    public void outOfPumpkinContact()
    {
        pumpkin = null;
    }

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
