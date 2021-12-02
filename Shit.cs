using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shit : MonoBehaviour
{
    public int NewVegas = 1488;
    public float speed = 1f; // ñêîðîñòü
    public int lives = 5; // êîëè÷åñòâî æèçíåé
    private float jumpForce = 3.5f; // ñèëíà ïðûæêà
    private bool isGrounded = false; // 1 ïðûæîê 
    public Transform player;
    private Animator anim;
    private SpriteRenderer sprite;


    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        CheckGround();
    }
    // Update is called once per frame
    void Update()
    {
        Run();
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime);


    }
    public Rigidbody2D rb;

    public void GetDamage(int damage)
    {
        lives -= damage;

    }

    private void Die() // Smertj
    {
        Destroy(this.gameObject);
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            player.gameObject.GetComponent<Shit>().GetDamage(1);
            lives--;
            Debug.Log("у червяка" + lives);
        }
        if (lives < 1)
            Die();

    }

    private States State // ýòî àíèìàöèè
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        if (!player) // Camera
            player = gameObject.transform;
    }




    private void Run()
    {
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        if (dir.x == 0)
        {
            State = States.Animations;
        }
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 1f);
        isGrounded = false;


        foreach (var col in collider)
        {
            Debug.Log(col);
            if (col.gameObject.tag != "Player")
            {
                isGrounded = true;
            }
        }
        if (!isGrounded) State = States.Jump;
        if (isGrounded) State = States.run;
    }

    public enum States
    {
        Animations,
        run,
        Jump,
    }


}