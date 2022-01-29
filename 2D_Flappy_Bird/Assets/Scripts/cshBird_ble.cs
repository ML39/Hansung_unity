using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshBird_ble : MonoBehaviour
{
    public cshGameManager gameManager;
    public float velocity = 0.7f;
    private Rigidbody2D rb;
    public bool super;

    public float exhale;
    public float inhale;

    // Start is called before the first frame update
    void Start()
    {
        super = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        exhale = GameObject.Find("bleManager").GetComponent<StartingExample>().exhale;
        inhale = GameObject.Find("bleManager").GetComponent<StartingExample>().inhale;
        if (exhale >= 0.5f)
        {
            rb.velocity = Vector2.up * velocity;
        }
        else if (inhale >= 0.5f)
        {
            rb.velocity = Vector2.down * velocity;
        }
        else
        {
            rb.velocity = Vector2.up * 0.12f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (super)
            return;
        gameManager.GameOver();
    }
}
