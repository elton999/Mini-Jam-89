using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    Movement movement;
    [SerializeField] float minDistance;
    void Start()
    {
        movement = GetComponent<Movement>();
    }
    void Update()
    {
       
    }

    void FixedUpdate()
    {
         if(Vector3.Distance(transform.position, target.position) < minDistance)
            movement.direction = Vector3.zero;
         else
            updateTargetPosition();
    }

    void updateTargetPosition(){
        movement.direction =  target.position - transform.position;
        movement.direction.Normalize();
    }
}
