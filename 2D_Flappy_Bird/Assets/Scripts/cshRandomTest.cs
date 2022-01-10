using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshRandomTest : MonoBehaviour
{
    public static cshRandomTest instance = null;
    public int rndValue;
    public Text txt;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("randomValue", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void randomValue() 
    {
        rndValue = Random.Range(0, 100);
        txt.GetComponent<UnityEngine.UI.Text>().text = rndValue.ToString();
    }
}
