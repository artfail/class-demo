using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAdv : MonoBehaviour
{
    public int speed = 2;
    public float lookDist = 4;
    public LayerMask GroundWallLayer;
    Transform player;
    public Transform castPoint;

    public GameObject explosion;
    Animator _animator;
    Rigidbody2D _rigidbody;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        StartCoroutine(MoveLoop());
    }


    IEnumerator MoveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(.2f);
            if (Vector2.Distance(transform.position, player.position) < lookDist)
            {
                if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
                player.position.x < transform.position.x && transform.localScale.x > 0)
                {
                    transform.localScale *= new Vector2(-1, 1);
                }
            }
            else if (Physics2D.Raycast(castPoint.position, transform.forward, 1, GroundWallLayer) ||
            !Physics2D.Raycast(castPoint.position, -transform.up, 1, GroundWallLayer))
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _rigidbody.velocity = new Vector2(speed * transform.localScale.x, _rigidbody.velocity.y);
        }
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