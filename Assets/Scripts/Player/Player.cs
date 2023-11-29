using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    private float initialSpeed;
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    private float moveH,moveV;
    private bool isRight = true;
    public enum States {
        idle,
        walk,
        attack,
        takingDamage
    }
    public States currentState = States.walk;
    //Poderes
    public GameObject tornado, projectile;
    public float  tornadoXOffSet = 0.5f, tornadoYOffSet = -0.6f;
    public float  projectileXOffSet = 0.5f, projectileYOffSet = -0.3f;

    //UI
    public bool isRunning, canRun;
    public GameObject staminaBar, staminaBarContainer;
    public float stamina = 100;
    public float maxStamina = 100;
    public SpriteRenderer staminaBarSpriteRenderer;

    public RectTransform hpBar;
    public float hp;
    public float maxHp;
    public Image hpImage;
    public float recoilForce;

    void Start()
    {
        initialSpeed = speed;
        hp = maxHp;
    }
    void FixedUpdate()
    {
        Movement();
    }
    void Update()
    {
        Animations();
        Actions();
        Hp();
    }
    void Movement()
    {
        if(currentState == States.walk || currentState == States.idle)
        {
            moveH = Input.GetAxisRaw("Horizontal");
            moveV = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(moveH,moveV);
            rb.velocity = movement.normalized * speed;

            if(moveH > 0) isRight = true;
            if(moveH < 0) isRight = false;

            currentState = (moveH == 0 && moveV == 0) ? States.idle : States.walk;
        }
        else if(currentState != States.takingDamage)
        {
            rb.velocity = Vector2.zero;
        }
    }
    void Actions()
    {
        if(Input.GetKeyDown(KeyCode.Q) && (currentState == States.walk || currentState == States.idle))
        {
            StartCoroutine(TornadoAtack(0.75f));
        }
        if (Input.GetMouseButtonDown(0) && (currentState == States.walk || currentState == States.idle))
        {
            Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            isRight = mousePositionWorld.x > transform.position.x;
            StartCoroutine(ProjectileAtack(0.2f));
        }
        Run();
    }
    void Animations()
    {
        switch(currentState)
        {
            case States.idle: anim.SetInteger("int",isRight? 1 : 2); break;
            case States.walk: anim.SetInteger("int",isRight? 11 : 12); break;
            case States.attack: anim.SetInteger("int", isRight? 21 : 22); break;
            case States.takingDamage: anim.SetInteger("int", isRight? 31 : 32); break;
        }   
    }
    void Run()
    {
        canRun = stamina >= maxStamina;
        if(Input.GetKeyDown(KeyCode.Space) && currentState != States.attack && !isRunning && canRun)
        {
            isRunning = true;
        }
        if(!isRunning && stamina < maxStamina)
        {
            stamina += maxStamina / 100;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            isRunning = false;
        }
        if(isRunning && stamina > 0 && currentState != States.attack) 
        {
            stamina--;
        }
        if(stamina == 0)
        {
            isRunning = false;
        }
        if(stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        anim.speed = isRunning? 1.5f : 1;
        speed = isRunning? initialSpeed * 1.5f : initialSpeed;
        staminaBarSpriteRenderer.color = canRun? Color.green : isRunning? Color.yellow : Color.red;
        staminaBar.transform.localScale = new Vector3( stamina / maxStamina ,1,1);
    }
    //HP
    void Hp()
    {
        hpBar.localScale = new Vector3(hp / maxHp, 1 , 1);
        hpImage.color = (hp > (maxHp/2))? Color.green : (hp > (maxHp/4)? Color.yellow : Color.red);
    }
    
    //Atacks
    void Tornado()
    {
        GameObject _tornado = Instantiate(tornado, new Vector3(transform.position.x + (1 * (isRight ? tornadoXOffSet : -tornadoXOffSet) ),
        transform.position.y + tornadoYOffSet, transform.position.z), Quaternion.identity);
        _tornado.GetComponent<Tornado>().isRight = isRight;
    }
    IEnumerator TornadoAtack(float seconds)
    {
        currentState = States.attack;
        anim.SetInteger("int",isRight? 21 : 22);
        yield return new WaitForSeconds(seconds);
        Tornado();
        currentState = States.walk;
    }
    void Projectile()
    {
        GameObject _projectile = Instantiate(projectile, new Vector3(transform.position.x + (1 * (isRight? projectileXOffSet : -projectileXOffSet)),
        transform.position.y + projectileYOffSet, transform.position.z), Quaternion.identity);
        _projectile.GetComponent<Projectile>().isRight = isRight;
    }
    IEnumerator ProjectileAtack(float seconds)
    {
        currentState = States.attack;
        anim.SetInteger("int",isRight? 21 : 22);
        yield return new WaitForSeconds(seconds);
        Projectile();
        currentState = States.walk;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        switch(collider.gameObject.tag)
        {
            case "Carrot":  
            if(collider.GetComponent<Carrot>().isReady) 
            {
                collider.GetComponent<Carrot>().scaleFactor = 0;
                stamina += 25;
            }
            break;
            case "Enemy_1":
            if(currentState != States.takingDamage)
            {
                rb.AddForce(new Vector2((transform.position.x - collider.transform.position.x) * recoilForce,
                (transform.position.y - collider.transform.position.y) * recoilForce), ForceMode2D.Impulse);
                StartCoroutine(TakingDamage(0.25f, collider.gameObject.GetComponent<Enemy_1>().damage));
            }
            
            break;
        }
    }

    IEnumerator TakingDamage(float seconds, int damage)
    {
        currentState = States.takingDamage;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        yield return new WaitForSeconds(seconds);
        StartCoroutine(DecreaseHp(damage));
        currentState = States.idle;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }
    IEnumerator DecreaseHp(int damage)
    {
        int _damage = damage;
        while(_damage > 0)
        {
            hp--;
            yield return new WaitForSeconds(0.025f);
            _damage--;
        }
        yield return new WaitForSeconds(0.01f);
    }
}
