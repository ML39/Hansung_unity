using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class cshUserInfo : MonoBehaviour
{
    public static string Email;
    public static string LogINTime;
    public static int GameLevel;
    public static int GameScore;
    public static int GameTime;
    public static string GameStartTime;
    public static string GameOverTime;
    public static int Survey;
    public static int exhaleCnt;
    public static int inhaleCnt;
    public static float exhaleSpd;
    public static float inhaleSpd;
    public static List<Breath> BreathList = new List<Breath>();

    [System.Serializable]
    public class UserInfo
    {
        public string EmailInfo;
        public string LogINTimeInfo;
        public int GameLevelInfo;
        public int GameScoreInfo;
        public List<Breath> BreathInfo;
        public int GameTimeInfo;
        public string GameStartTimeInfo;
        public string GameOverTimeInfo;
        public int SurveyInfo;
    }

    [System.Serializable]
    public class Breath
    {
        public int cnt;
        public int exhaleCntInfo;
        public int inhaleCntInfo;
        public float exhaleSpdInfo;
        public float inhaleSpdInfo;
    }

    public class UserPost
    {
        public string SK = "playerName#LJMo_-#date#" + DateTime.Now;
        public string playerID = "ljm9802mo@naver.com";
        public List<UserInfo> playerInfo;
        public string PK = "PLAYER";
    }

    public class Users
    {
        public List<UserInfo> users = new List<UserInfo>();
    }

    private static cshUserInfo instance = null;
    public static cshUserInfo Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Users user_arr = new Users();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
