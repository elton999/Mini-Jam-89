using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Movement))]
public class SpriteDirection : MonoBehaviour
{
    Movement movement;
    SpriteRenderer spriteRenderer;
    void Awake()
    {
        movement = GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        spriteRenderer.flipX = movement.direction.x > 0;
    }
}
