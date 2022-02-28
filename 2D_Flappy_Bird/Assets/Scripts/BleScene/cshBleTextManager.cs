using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshBleTextManager : MonoBehaviour
{
    public Text StatusText, ButtonPositionText;
    public GameObject bleManager;

    // Start is called before the first frame update
    void Start()
    {
        bleManager = GameObject.Find("bleManager");
    }
    // Update is called once per frame
    void Update()
    {
        StatusText.text = bleManager.GetComponentsInChildren<Text>()[0].text;
        ButtonPositionText.text = bleManager.GetComponentsInChildren<Text>()[1].text;
    }
}
