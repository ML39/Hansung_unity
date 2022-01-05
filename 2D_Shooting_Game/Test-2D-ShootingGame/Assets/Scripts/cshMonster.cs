using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshMonster : MonoBehaviour
{
    public float moveSpeed = 1f;

    public Rigidbody2D rb;
    public Animator animator;
    public GameObject dieEffect;

    public Transform target;

    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTarget();

        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            GameObject effect = Instantiate(dieEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
            Destroy(gameObject);
            GameObject.Find("Player").GetComponent<cshScore>().score += 500;
        }
    }

    public void CheckTarget() 
    {
        float x = target.position.x - rb.position.x;
        float y = target.position.y - rb.position.y;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            movement.y = 0;
            if (x < 0)
            {
                movement.x = -1;
            }
            else
            {
                movement.x = 1;
            }
        }
        else 
        {
            movement.x = 0;
            if (y < 0)
            {
                movement.y = -1;
            }
            else
            {
                movement.y = 1;
            }
        }
    }
}
