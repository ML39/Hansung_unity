using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshCoinSpawner : MonoBehaviour
{
    public float maxTime = 2;
    private float timer = 0;
    public GameObject coin;
    public float height;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > maxTime)
        {
            GameObject newcoin = Instantiate(coin);
            newcoin.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
            Destroy(newcoin, 10);
            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
