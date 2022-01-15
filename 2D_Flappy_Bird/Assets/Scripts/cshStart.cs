using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cshStart : MonoBehaviour
{
    public static int level = 2;

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
        SceneManager.LoadScene("GameScene");
    }

    public void EasyGame()
    {
        level = 1;
    }
    public void NormalGame()
    {
        level = 2;
    }
}
