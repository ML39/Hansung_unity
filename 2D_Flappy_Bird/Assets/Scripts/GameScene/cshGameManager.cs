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

    public int level;

    public float GameStartTime;
    public float GameOverTime;
    public float GameTime;

    // Start is called before the first frame update
    void Start()
    {
        cshEventManager.Instance.AddListener(EVENT_TYPE.LEVEL_SELECT, GameEvent);
        cshEventManager.Instance.AddListener(EVENT_TYPE.GAME_START, GameEvent);
        cshEventManager.Instance.AddListener(EVENT_TYPE.GAME_OVER, GameEvent);
        cshEventManager.Instance.AddListener(EVENT_TYPE.GAME_RESULT, GameEvent);

        level = cshStart.level;
        Bird.GetComponent<Animator>().SetInteger("Level", level);
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
        Time.timeScale = 1;

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

        cshEventManager.Instance.PostNotification(EVENT_TYPE.GAME_START, this);
        cshEventManager.Instance.PostNotification(EVENT_TYPE.LEVEL_SELECT, this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == EVENT_TYPE.GAME_START)
        {
            GameStartTime = Time.time;
            Debug.Log("StartGame" + ", Time : " + DateTime.Now);
        }
        else if (Event_Type == EVENT_TYPE.GAME_OVER)
        {
            GameOverTime = Time.time;
            Time.timeScale = 0;
            cshEventManager.Instance.PostNotification(EVENT_TYPE.GAME_RESULT, this);
        }
        else if (Event_Type == EVENT_TYPE.LEVEL_SELECT)
        {
            
        }
        else if (Event_Type == EVENT_TYPE.GAME_RESULT)
        {
            GameTime = GameOverTime - GameStartTime;
            cshEventManager.GameTime = (int)GameTime;
            cshEventManager.GameScore = cshScore.score;
            cshEventManager.GameDateTime = DateTime.Now.ToString("G");

            Debug.Log("GameOver, Score:" + cshScore.score + ", Time : " + DateTime.Now);

            cshEventManager.check = true;
        }
    }

    public void LevelSelect() 
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
        cshEventManager.Instance.PostNotification(EVENT_TYPE.GAME_OVER, this);
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void Replay()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Menu()
    {
        cshEventManager.Instance.PostNotification(EVENT_TYPE.GAME_OVER, this);
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
