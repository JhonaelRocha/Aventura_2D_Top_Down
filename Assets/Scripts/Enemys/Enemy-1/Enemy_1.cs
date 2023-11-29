using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public bool isRight;
    public GameObject player;
    public float speed;
    public float distanceToAttack, distanceToSpawn;
    private bool isReady;
    public bool isDead;
    public float _scaleFactorToDeath = 100;
    private float _startScaleFactorToDeath;

    public int damage;
    void Start()
    {
        _startScaleFactorToDeath = _scaleFactorToDeath;
        anim = GetComponent<Animator>();
        anim.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        isRight = transform.position.x < player.transform.position.x;
        Animations();
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if(distanceToPlayer < distanceToSpawn && anim.GetInteger("state") == 0 && !isReady)
        {
            isReady = true;
            anim.speed = 1;
            StartCoroutine(Spawn(1.25f));
        }
        if(distanceToPlayer < distanceToAttack && anim.GetInteger("state") == 1 
        && player.transform.position.y == Mathf.Clamp(player.transform.position.y,transform.position.y - 0.5f,transform.position.y + 0.5f))
        {
            anim.SetInteger("state", 2);
            StartCoroutine(Attack(0.75f));
        }
        if(!(transform.position.x == Mathf.Clamp(transform.position.x, -10, 10)))
        {
            Destroy(this.gameObject);
        }
        if(isDead)
        {
            rb.velocity = new Vector3(0,0,0);
            if(_scaleFactorToDeath > 0)
            {
                _scaleFactorToDeath--;
            }
            if(_scaleFactorToDeath <= 0)
            {
                Destroy(this.gameObject);
            }
            transform.localScale = new Vector3(_scaleFactorToDeath/_startScaleFactorToDeath,_scaleFactorToDeath/_startScaleFactorToDeath,_scaleFactorToDeath/_startScaleFactorToDeath);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Solid": 
            rb.velocity = new Vector3(0,0,0);
            if(anim.GetInteger("state") == 3)
            {
                isDead = true;
            }
            break;
        }
    }
    
    void Animations()
    {
        anim.SetBool("isRight", isRight);
    }
    IEnumerator Spawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        anim.SetInteger("state", 1);
    }
    IEnumerator Attack(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        rb.velocity = new Vector3 (isRight? 1 * speed : -1 * speed, 0, 0);
        anim.SetInteger("state",3);
    }
}
