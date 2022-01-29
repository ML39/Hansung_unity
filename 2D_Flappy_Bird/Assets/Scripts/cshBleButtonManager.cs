using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshBleButtonManager : MonoBehaviour
{
    public Button subscribe, disconnect, rescan;
    public GameObject bleManager;

    // Start is called before the first frame update
    void Start()
    {
        bleManager = GameObject.Find("bleManager");
        subscribe.onClick.AddListener(bleManager.GetComponent<StartingExample>().Subscribe);
        disconnect.onClick.AddListener(bleManager.GetComponent<StartingExample>().Disconnect);
        rescan.onClick.AddListener(bleManager.GetComponent<StartingExample>().Scan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
