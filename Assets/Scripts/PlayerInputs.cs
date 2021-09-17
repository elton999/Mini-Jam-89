using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] Movement movement;
    void Update()
    {
        input();
    }

    void input(){
        movement.direction = Vector3.zero;
        float horizontal = (float)Math.Round(Input.GetAxis("Horizontal"));
        float vertical = (float)Math.Round(Input.GetAxis("Vertical"));

        movement.direction = new Vector3(Math.Sign(horizontal), Math.Sign(vertical), 0);
    }
}
