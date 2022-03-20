using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshHardPipeSpawner : MonoBehaviour
{
    private float timer = 0;
    public GameObject pipe;
    public float height;

    public int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > cshValueSetting.HARDmaxTime)
        {
            GameObject newpipe = Instantiate(pipe);
            newpipe.transform.position = transform.position + new Vector3(0, cshValueSetting.HARD_spawnPosition[i], 0);
            Destroy(newpipe, 20);
            timer = 0;

            i += 1;
            if (i >= cshValueSetting.HARD_spawnPosition.Length)
                i = 0;
        }

        timer += Time.deltaTime;
    }
}
