using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cshScene2Scene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void Back2Ble()
    {
        SceneManager.LoadScene("ble_test_scence1");
    }

    public void Preferences()
    {
        SceneManager.LoadScene("PreferencesScene");
    }
}
