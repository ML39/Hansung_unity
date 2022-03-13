using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshMinusScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        cshEventManager.Instance.AddListener(EVENT_TYPE.SCORE_MINUS, ScoreMinus);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cshEventManager.Instance.PostNotification(EVENT_TYPE.SCORE_MINUS, this, cshScore.score);
        this.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public void ScoreMinus(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        int score = (int)Param;
        if (score > 0)
            score -= 1;
        else
            score = 0;
        cshScore.score = score;
    }
}
