using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshAddScore : MonoBehaviour
{
    public GameObject PointEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cshScore.score += 1;
        GameObject.Find("SoundManager").GetComponent<cshSound>().check = 1;
        GameObject effect = Instantiate(PointEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.4f);
        Destroy(gameObject);
    }
}
