using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] Movement movement;
    void Update()
    {
        if(movement.getHit) return;
        input();
    }

    void input(){
        movement.direction = Vector3.zero;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement.direction = new Vector3(horizontal, vertical, 0);
        movement.direction.Normalize();
    }
}
