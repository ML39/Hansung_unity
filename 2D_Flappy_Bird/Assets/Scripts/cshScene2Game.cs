using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cshScene2Game : MonoBehaviour
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
        GameObject.Find("StatusText").SetActive(false);
        GameObject.Find("ButtonPositionText").SetActive(false);
    }
}
