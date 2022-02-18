using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cshGameManager : MonoBehaviour
{
    public GameObject PauseCanvas;
    public GameObject gameOverCanvas;
    public GameObject ScoreCanvas;
    public GameObject CoinSpawner;
    public GameObject NormalPipeSpawner;
    public GameObject HardPipeSpawner;
    public GameObject Bird;
    public GameObject Ready;

    public int level;

    // Start is called before the first frame update
    void Start()
    {
        level = cshStart.level;
        Bird.GetComponent<Animator>().SetInteger("Level", level);
        Time.timeScale = 1;

        switch (level)
        {
            case 1:
                Easy();
                break;
            case 2:
                Normal();
                break;
            case 3:
                Hard();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }


    public void Replay()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }

    public void Easy()
    {
        CoinSpawner.SetActive(true);
        GameObject.Find("Bird").GetComponent<cshBird>().super = true;
        GameObject.Find("Bird").GetComponent<cshBird_ble>().super = true;
    }

    public void Normal()
    {
        NormalPipeSpawner.SetActive(true);
    }

    public void Hard()
    {
        HardPipeSpawner.SetActive(true);
    }
}
