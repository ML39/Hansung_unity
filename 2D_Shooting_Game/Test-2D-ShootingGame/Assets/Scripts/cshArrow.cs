using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshArrow : MonoBehaviour
{
    public GameObject hitEffect;

    private void Start()
    {

    }

    private void Update()
    {
        Destroy(gameObject, 1.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) 
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
            Destroy(gameObject);
        }
    }
}
