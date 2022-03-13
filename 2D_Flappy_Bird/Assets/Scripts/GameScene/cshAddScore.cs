using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshAddScore : MonoBehaviour
{
    public GameObject PointEffect;
    // Start is called before the first frame update
    void Start()
    {
        cshEventManager.Instance.AddListener(EVENT_TYPE.SCORE_PLUS, ScorePlus);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cshEventManager.Instance.PostNotification(EVENT_TYPE.SCORE_PLUS, this, cshScore.score);
        GameObject.Find("SoundManager").GetComponent<cshSound>().check = 1;
        GameObject effect = Instantiate(PointEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.4f);
        Destroy(gameObject);
    }

    public void ScorePlus(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        int score = (int)Param;
        score += 1;
        cshScore.score = score;
    }
}
