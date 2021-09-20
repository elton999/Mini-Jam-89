using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Movement movement;
    Rigidbody2D rigidBody;
    Animator animator;
    public static Player Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();    
        animator = GetComponent<Animator>();
    }

    void Update() {
        if(Vector3.Distance(movement.direction, new Vector3(0,0,movement.direction.z)) != 0)
            animator.Play("walk");
        else
            animator.Play("idle");
    }
    void FixedUpdate()
    {
        rigidBody.velocity = Vector2.zero;
    }
}
