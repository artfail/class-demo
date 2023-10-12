using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegamanMove : MonoBehaviour
{
    public int speed = 5;
    public int jumpForce = 700;

    public bool hurt = false;
    public float recoveryTime = .3f;
    private int dir = 1;

    public bool grounded;
    public LayerMask ground;
    public Transform feet;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer rend;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        float xSpeed = Input.GetAxis("Horizontal") * speed;

        if (!hurt)
        {
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(xSpeed));
        }

        if ((xSpeed < 0 && transform.localScale.x > 0) || (xSpeed > 0 && transform.localScale.x < 0))
        {
            transform.localScale *= new Vector2(-1, 1);
            dir *= -1;
        }

        grounded = Physics2D.OverlapCircle(feet.position, .3f, ground);
        anim.SetBool("Grounded", grounded);
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Shoot");

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !hurt)
        {
            StartCoroutine(IFrames());
        }
    }

    IEnumerator IFrames()
    {
        hurt = true;
        anim.SetBool("Hurt", hurt);
        rb.AddForce(new Vector2(-dir * 200, 100));
        rend.color = new Color(1, .5f, .5f);

        yield return new WaitForSeconds(recoveryTime);
        hurt = false;
        anim.SetBool("Hurt", hurt);
        rend.color = Color.white;
    }
}
