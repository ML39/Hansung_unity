using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshSpawnMonster : MonoBehaviour
{
    public GameObject monster;
    public Transform[] spawnPoints;
    float nextSpawn = 0.0f;
    float spawn = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn && GameObject.Find("Player")) 
        {
            nextSpawn = Time.time + spawn;
            for(int i = 0; i < 5; i++)
            {
                Instantiate(monster, spawnPoints[Random.Range(0, spawnPoints.Length)].position
                    , Quaternion.identity);
            }
        }
    }
}
