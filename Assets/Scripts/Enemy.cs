using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    Movement movement;
    void Start()
    {
        movement = GetComponent<Movement>();
    }
    void Update()
    {
        if(Vector3.Distance(transform.position, target.position) < 0.5f)
            movement.direction = Vector3.zero;
    }

    void FixedUpdate()
    {
        updateTargetPosition();
    }

    void updateTargetPosition(){
        movement.direction =  target.position - transform.position;
        movement.direction.Normalize();
    }
}
