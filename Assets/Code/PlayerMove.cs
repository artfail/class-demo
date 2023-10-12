using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    public int speed = 4;
    public int jumpForce = 800;
    Rigidbody2D _rigidbody;
    Animator _animator;
    SpriteRenderer _renderer;

    public LayerMask groundLayer;
    public Transform feetPoint;
    public bool grounded;

    private bool justHit = false;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }


    void FixedUpdate()
    {
        float xSpeed = Input.GetAxisRaw("Horizontal") * speed;
        if (!justHit)
        {
            _rigidbody.velocity = new Vector2(xSpeed, _rigidbody.velocity.y);
        }

        _animator.SetFloat("Speed", Mathf.Abs(xSpeed));

        if ((xSpeed < 0 && transform.localScale.x > 0) || (xSpeed > 0 && transform.localScale.x < 0))
        {
            transform.localScale *= new Vector2(-1, 1);
        }
    }

    void Update()
    {

        grounded = Physics2D.OverlapCircle(feetPoint.position, .305f, groundLayer);
        if (Input.GetButtonDown("Jump") && grounded)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
            _rigidbody.AddForce(new Vector2(0, jumpForce));
        }
        _animator.SetBool("Grounded", grounded);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !justHit)
        {
            StartCoroutine(IFrames());
        }
    }

    IEnumerator IFrames()
    {
        justHit = true;
        _rigidbody.AddForce(new Vector2(transform.localScale.x * -300, 300));
        _renderer.color = Color.red;

        yield return new WaitForSeconds(.3f);
        _renderer.color = Color.white;
        justHit = false;
    }
}