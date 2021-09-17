using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [NonSerializedAttribute] public Vector3 direction;
    [SerializeField] float speed = 1.0f;
    void Start()
    {
        direction = Vector3.zero;
    }

    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
    }
}
