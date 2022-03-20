using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshBird_ble : MonoBehaviour
{
    public cshGameManager gameManager;
    private Rigidbody2D rb;
    //public bool super;

    public float exhale;
    public float inhale;

    // Start is called before the first frame update
    void Start()
    {
        //super = false;
        rb = GetComponent<Rigidbody2D>();

        cshEventManager.Instance.AddListener(EVENT_TYPE.CHECK_EXHALE_SPD, checkSPD);
        cshEventManager.Instance.AddListener(EVENT_TYPE.CHECK_INHALE_SPD, checkSPD);
    }

    // Update is called once per frame
    void Update()
    {
        exhale = GameObject.Find("bleManager").GetComponent<StartingExample>().exhaleSpd;
        inhale = GameObject.Find("bleManager").GetComponent<StartingExample>().inhaleSpd;

        if (exhale >= inhale)
            cshEventManager.Instance.PostNotification(EVENT_TYPE.CHECK_EXHALE_SPD, this, exhale);
        else if (exhale < inhale)
            cshEventManager.Instance.PostNotification(EVENT_TYPE.CHECK_INHALE_SPD, this, inhale);

        if (exhale >= 0.5f && exhale >= inhale)
        {
            rb.velocity = new Vector2(0, cshValueSetting.speed);
        }
        else if (inhale >= 0.5f && exhale < inhale)
        {
            rb.velocity = new Vector2(0, -cshValueSetting.speed);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (super)
            return;
        //cshEventManager.Instance.PostNotification(EVENT_TYPE.GAME_OVER, gameManager);
    }
    */

    public void checkSPD(EVENT_TYPE Event_Type, Component Sender, object Param = null) 
    {
        if (Event_Type == EVENT_TYPE.CHECK_EXHALE_SPD || Event_Type == EVENT_TYPE.CHECK_INHALE_SPD)
        {
            if ((float)Param >= 1.0f)
            {
                cshGameManager.Check = true;
            }
            else if ((float)Param < 1.0f)
            {
                cshGameManager.Check = false;
            }
        }
    }
}
