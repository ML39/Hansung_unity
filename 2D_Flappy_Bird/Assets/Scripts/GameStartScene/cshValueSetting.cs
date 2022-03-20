using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshValueSetting : MonoBehaviour
{
    public static float[] EASY_spawnPosition = { 0.2f, 0.6f, 0.4f, -0.2f, -0.6f, -0.4f };
    public static float[] NORMAL_spawnPosition = { 0.2f, 0.6f, 0.4f, -0.2f, -0.6f, -0.4f };
    public static float[] HARD_spawnPosition = { 0.2f, 0.6f, 0.4f, -0.2f, -0.6f, -0.4f };

    public static float EASYmaxTime = 6;
    public static float NORMALmaxTime = 6;
    public static float HARDmaxTime = 6;

    public static float EASYspeed = 0.3f;
    public static float NORMALspeed =0.3f;
    public static float HARDspeed = 0.3f;

    public static float speed = 1;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
