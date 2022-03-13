using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshPipeSpawner : MonoBehaviour
{
    public float maxTime = 6;
    private float timer = 0;
    public GameObject pipe;
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
            GameObject newpipe = Instantiate(pipe);
            newpipe.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
            Destroy(newpipe, 20);
            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
