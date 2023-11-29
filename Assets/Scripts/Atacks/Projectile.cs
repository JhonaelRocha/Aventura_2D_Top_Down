using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isRight;
    public float speed;
    public Rigidbody2D rb;
    private Vector2 movement;
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer.flipX = isRight? false : true;
        movement = isRight? Vector2.right : Vector2.left;
    }
    void FixedUpdate()
    {
        rb.velocity = movement.normalized * speed;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        switch(collider.gameObject.tag)
        {
            case "Tornado" :
            Debug.Log("Colidiu");
            break;
            case "Enemy_1":
            if(collider.GetComponent<Enemy_1>().anim.GetInteger("state") >= 1)
            {
                collider.GetComponent<Enemy_1>().isDead = true;
            }
            break;
        }
    }
}
