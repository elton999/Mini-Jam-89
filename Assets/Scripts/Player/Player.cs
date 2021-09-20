using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Movement movement;
    Rigidbody2D rigidBody;
    Animator animator;
    public bool isAttacking = false;
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
        Animation();
    }

    void Animation(){
        if(movement.direction.x > 0)
            animator.Play("walk_right");
        else if(movement.direction.x < 0)
            animator.Play("walk_left");
        else if(movement.direction.y > 0)
            animator.Play("walk_up");
        else if(movement.direction.y < 0)
            animator.Play("walk_down");
        else
            animator.Play("idle");
    }
    void FixedUpdate()
    {
        rigidBody.velocity = Vector2.zero;
    }
}
