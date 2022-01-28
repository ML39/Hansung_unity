using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshDontDestroyObject : MonoBehaviour
{
    private void Awake()
    { 
        DontDestroyOnLoad(GameObject.Find("bleManager")); 
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
