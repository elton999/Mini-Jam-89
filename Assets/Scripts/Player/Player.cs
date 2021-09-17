using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Movement movement;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Enemy"))
            movement.takeHit(
                movement.direction == Vector3.zero ? -other.gameObject.GetComponent<Movement>().direction : movement.direction
            );
    }
}
