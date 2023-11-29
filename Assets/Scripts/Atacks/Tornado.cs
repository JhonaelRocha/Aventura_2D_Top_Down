using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public bool isRight;
    public float speed;
    public Rigidbody2D rb;
    private Vector2 movement;
    void Start()
    {
        movement = isRight? Vector2.right : Vector2.left;
    }
    void FixedUpdate()
    {
        rb.velocity = movement.normalized * speed;
    }
}
