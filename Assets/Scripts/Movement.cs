using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [NonSerializedAttribute] public Vector3 direction;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float hitForce = 2.5f;
    [SerializeField] float delayAfterHit = 3.0f;
    [NonSerializedAttribute] public bool getHit = false;
    Rigidbody2D rigidBody;
    void Start()
    {
        direction = Vector3.zero;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(getHit) return;
        transform.position += direction * Time.deltaTime * speed;
    }

    public void takeHit(Vector3 newDirection)
    {
        if(getHit) return;
        getHit = true;
        rigidBody.AddRelativeForce(newDirection * - hitForce);
        Invoke("backToCombat", delayAfterHit);  
    }

    void backToCombat(){
        getHit = false;
        rigidBody.velocity = Vector2.zero;
    }
}
