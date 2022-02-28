using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

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

    private UnityAction GameStartListener;
    private UnityAction GameOverListener;

    public float GameStartTime;
    public float GameOverTime;
    public float GameTime;

    private void Awake()
    {
        GameStartListener = new UnityAction(GameStartEvent);
        GameOverListener = new UnityAction(GameOverEvent);
    }

    private void OnEnable()
    {
        cshEventManager.StartListening(EVENT_TYPE.GAMESTART, GameStartListener);
        cshEventManager.StartListening(EVENT_TYPE.GAMEOVER, GameOverListener);
    }

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

        cshEventManager.TriggerEvent(EVENT_TYPE.GAMESTART);

        if (cshTMP.blechecktmp == true)
        {
            Bird.GetComponent<cshBird_ble>().enabled = true;
            Bird.GetComponent<cshBird>().enabled = false;
        }
        else if (cshTMP.blechecktmp == false)
        {
            Bird.GetComponent<cshBird_ble>().enabled = false;
            Bird.GetComponent<cshBird>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStartEvent()
    {
        GameStartTime = Time.time;
        Debug.Log("StartGame" + ", Time : " + DateTime.Now);
    }

    public void GameOverEvent() 
    {
        GameOverTime = Time.time;
        GameTime = GameOverTime - GameStartTime;
        cshEventManager.GameTime = (int)GameTime;
        cshEventManager.GameScore = cshScore.score;
        cshEventManager.GameDateTime = DateTime.Now.ToString("G");

        Debug.Log("GameOver, Score:"+ cshScore.score + ", Time : " + DateTime.Now);
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
        cshEventManager.TriggerEvent(EVENT_TYPE.GAMEOVER);
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }


    public void Replay()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Menu()
    {
        cshEventManager.TriggerEvent(EVENT_TYPE.GAMEOVER);
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
