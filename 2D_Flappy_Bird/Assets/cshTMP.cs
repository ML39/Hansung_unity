using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshTMP : MonoBehaviour
{
    public static bool blechecktmp = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void tmp() 
    {
        if (blechecktmp == true)
            blechecktmp = false;
        else if (blechecktmp == false)
            blechecktmp = true;
    }
}
