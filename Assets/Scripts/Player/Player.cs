using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool hasFlag = false;
    [SerializeField] Movement movement;
    Rigidbody2D rigidBody;
    public static Player Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();    
    }
    void FixedUpdate()
    {
        rigidBody.velocity = Vector2.zero;
    }
}
