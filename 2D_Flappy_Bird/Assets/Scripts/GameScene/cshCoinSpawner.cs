using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshCoinSpawner : MonoBehaviour
{
    private float timer = 0;
    public GameObject coin;
    public float height;

    public int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > cshValueSetting.EASYmaxTime)
        {
            GameObject newcoin = Instantiate(coin);
            newcoin.transform.position = transform.position + new Vector3(0, cshValueSetting.EASY_spawnPosition[i], 0);
            Destroy(newcoin, 20);
            timer = 0;

            i += 1;
            if (i >= cshValueSetting.EASY_spawnPosition.Length)
                i = 0;
        }

        timer += Time.deltaTime;
    }
}
