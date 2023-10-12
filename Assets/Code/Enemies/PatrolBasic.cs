using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBasic : MonoBehaviour
{
    public float speed = 1f;
    public float distance = 5;

    private float startX;
    private float endX;
    private int dir = -1;

    public GameObject explosion;
    Animator _animator;
    Rigidbody2D _rigidbody;


    void Start()
    {
        startX = transform.position.x;
        endX = startX + distance;

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        /*
        if (transform.position.x < startX && dir == -1)
        {
            dir = 1;
        }
        if (transform.position.x > endX && dir == 1)
        {
            dir = -1;
        }
        */

        if ((transform.position.x < startX && dir == -1) || (transform.position.x > endX && dir == 1))
        {
            dir *= -1;
            transform.localScale *= new Vector2(dir, 1);
        }

        _rigidbody.velocity = new Vector2(dir * speed, _rigidbody.velocity.y);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _animator.SetTrigger("Die");
            Destroy(gameObject, .15f);
        }
    }
}