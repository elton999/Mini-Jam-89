using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Movement))]
public class SpriteDirection : MonoBehaviour
{
    Movement movement;
    SpriteRenderer spriteRenderer;
    Vector3 lastDirection;
    void Awake()
    {
        movement = GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if( Vector3.Distance(movement.direction, Vector3.zero) != 0)
            lastDirection = movement.direction;
        spriteRenderer.flipX = lastDirection.x > 0;
    }
}
