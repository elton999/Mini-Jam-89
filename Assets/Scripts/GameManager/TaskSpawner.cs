using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSpawner : MonoBehaviour
{

    public GameObject waterTaskBubble; // water, on pumpkin, all days except combat
    public GameObject pruneTaskBubble; // prune, on outer vines, all days except combat
    public GameObject unholyTaskBubble; // fertilize, on pumpkin, 1 day before combat
    public GameObject holyTaskBubble; // repair the damage dealt by the ghosts, on vines OR pumpkin, 1-2 days after combat

    // Enemies move toward the pumpkin
    // // Pause to attack and destroy encountered vines
    // Fewer vines means less pumpkin growth (unless vines are overgrown)
    // If the ghost reaches the pumpkin, it sucks size away from the pumpkin
    // // Pumpkin anims are only updated between days
    // Tools can be used regardless of tasks. But when used near a task, it deletes that task for 
    // // This enables overwatering / overpruning (less growth), and overfertilizing (more combat days)
    // Pumpkin spawns more tasks as it gets bigger

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
