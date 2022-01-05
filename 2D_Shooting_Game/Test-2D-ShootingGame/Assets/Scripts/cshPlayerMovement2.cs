using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshPlayerMovement2 : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    public Joystick joystick;

    public Text txt;

    Vector2 movement;
    void Start()
    {
        txt = GameObject.Find("GameOver").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("hori: "+joystick.Horizontal);
        Debug.Log("vert: " + joystick.Vertical);
        if (joystick.Horizontal > 0.5f)
        {
            movement.x = 1;
        }
        else if (joystick.Horizontal < -0.5f)
        {
            movement.x = -1;
        }
        else 
        {
            movement.x = 0;
        }
        if (joystick.Vertical > 0.5f)
        {
            movement.y = 1;
        }
        else if (joystick.Vertical < -0.5f)
        {
            movement.y = -1;
        }
        else 
        {
            movement.y = 0;
        }

        if (movement != Vector2.zero)
        {
            animator.SetBool("Shoot", false);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            //rb.isKinematic = false;
        }
        if (collision.gameObject.CompareTag("Monster"))
        {
            Destroy(gameObject);
            txt.text = "GameOver";
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
