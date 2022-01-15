using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cshGameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public GameObject ScoreCanvas;
    public GameObject PipeSpawner;
    public GameObject CoinSpawner;
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
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void Normal()
    {
        PipeSpawner.SetActive(true);
    }
}
