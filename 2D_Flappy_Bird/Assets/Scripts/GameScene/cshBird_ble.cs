using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshBird_ble : MonoBehaviour
{
    public cshGameManager gameManager;
    public float velocity = 0.3f;
    private Rigidbody2D rb;
    public bool super;

    public float exhale;
    public float inhale;

    // Start is called before the first frame update
    void Start()
    {
        super = false;
        rb = GetComponent<Rigidbody2D>();

        cshEventManager.Instance.AddListener(EVENT_TYPE.CHECK_EXHALE_SPD, checkSPD);
        cshEventManager.Instance.AddListener(EVENT_TYPE.CHECK_INHALE_SPD, checkSPD);
    }

    // Update is called once per frame
    void Update()
    {
        exhale = GameObject.Find("bleManager").GetComponent<StartingExample>().exhale;
        inhale = GameObject.Find("bleManager").GetComponent<StartingExample>().inhale;
        exhale = 1.5f;
        if (exhale >= 0.3f)
        {
            rb.velocity = Vector2.up * velocity;
            if (exhale >= 1.0f)
            {
                cshEventManager.Instance.PostNotification(EVENT_TYPE.CHECK_EXHALE_SPD, this, exhale);
            }
        }
        else if (inhale >= 0.3f)
        {
            rb.velocity = Vector2.down * velocity;
            if (inhale >= 1.0f)
            {
                cshEventManager.Instance.PostNotification(EVENT_TYPE.CHECK_INHALE_SPD, this, inhale);
            }
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
        cshEventManager.Instance.PostNotification(EVENT_TYPE.GAME_OVER, gameManager);
    }

    public void checkSPD(EVENT_TYPE Event_Type, Component Sender, object Param = null) 
    {
        if (Event_Type == EVENT_TYPE.CHECK_EXHALE_SPD)
        {
            Debug.Log((float)Param);
        }
        else if (Event_Type == EVENT_TYPE.CHECK_INHALE_SPD)
        {
            Debug.Log((float)Param);
        }
    }
}
