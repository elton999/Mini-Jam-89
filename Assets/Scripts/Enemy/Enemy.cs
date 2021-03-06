using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public TaskSpawner tasks; // gets assigned by SpawnEnemies, SpawnEnemiesManager
    // todo:
    // // when a vine is damaged, fill up TaskSpawner.enemyDamagePoints
    // // (over the next days, repair tasks will be spawned on these locations)
    // enemies just tint the vines to show dmg

    // enemies can dmg vines within a range, and at a frequency.

    public Transform target;
    Movement movement;
    [SerializeField] float minDistance;
    [SerializeField] float minDistanceFromPlayer;
    bool isRunAway = false;

    public AudioSource enemyAS;
    public AudioClip pumpkinSuck;
    public AudioClip[] scareAway;
    void Start()
    {
        movement = GetComponent<Movement>();
    }
    void FixedUpdate()
    {
        if(isRunAway) return;
        if(Vector3.Distance(transform.position, target.position) < minDistance)
            movement.direction = Vector3.zero;
        else
            updateTargetPosition();
        checkPlayerDistance();
    }

    void checkPlayerDistance(){
        var playerPosition = Player.Instance.transform.position;
        var holyPotionFlag = Player.Instance.GetComponent<Inventory>().holyPotionFlag;
        if(Vector3.Distance(playerPosition, transform.position) < minDistanceFromPlayer && holyPotionFlag){
            runAway(playerPosition);
        }
    }

    public void runAway(Vector3 position){
        isRunAway = true;
        movement.direction = -(position - transform.position).normalized;
        GetComponent<Collider2D>().enabled = false;
    }

    void OnBecameInvisible() {
        if(isRunAway){
            isRunAway = false;
            hitPumpkin = false;
            GetComponent<Collider2D>().enabled = true;
            gameObject.SetActive(false);
        }
    }

    void updateTargetPosition(){
        movement.direction =  target.position - transform.position;
        movement.direction.Normalize();
    }

    public bool hitPumpkin = false;
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Pumpkin") && !hitPumpkin){
            //other.gameObject.GetComponent<Pumpkin>().enemysInPumpkin++;
            hitPumpkin = true;
            enemyAS.PlayOneShot(pumpkinSuck);
        }

        if(other.collider.CompareTag("Player")){
            if(Player.Instance.isAttacking){
                runAway(target.position);
                enemyAS.PlayOneShot(scareAway[Random.Range(0, scareAway.Length)]);
            }
        } 
    }
}
