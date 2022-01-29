using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshDontDestroyObject : MonoBehaviour
{
    public static cshDontDestroyObject Tmp;

    private void Awake()
    {
        if (Tmp != null)
        {
            Destroy(gameObject);
            return;
        }

        Tmp = this;
        DontDestroyOnLoad(gameObject);
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
