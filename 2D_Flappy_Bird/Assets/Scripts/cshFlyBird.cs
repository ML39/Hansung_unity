using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshFlyBird : MonoBehaviour
{
    public cshGameManager gameManager;
    public float velocity = 1.4f;
    private Rigidbody2D rb;
    public bool super;

    // Start is called before the first frame update
    void Start()
    {
        super = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            rb.velocity = Vector2.up * velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (super)
            return;
        gameManager.GameOver();
    }
}
