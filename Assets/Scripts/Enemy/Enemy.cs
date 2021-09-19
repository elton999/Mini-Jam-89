﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    Movement movement;
    [SerializeField] float minDistance;
    [SerializeField] float minDistanceFromPlayer;
    bool isRunAway = false;
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
    }

    void OnBecameInvisible() {
        if(isRunAway){
            isRunAway = false;
            gameObject.SetActive(false);
        }
    }

    void updateTargetPosition(){
        movement.direction =  target.position - transform.position;
        movement.direction.Normalize();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
         //if(other.collider.CompareTag("Player"))
         //   movement.takeHit(-other.gameObject.GetComponent<Movement>().direction);    
    }
}
