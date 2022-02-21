using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System;

public class cshEventManager : MonoBehaviour
{
    public static string Email;
    public static int GameScore;
    public static int GameTime;
    public static string GameDateTime;
    public class UserInfo
    {
        public string EmailInfo;
        public int GameScoreInfo;
        public int GameTimeInfo;
        public string GameDateTimeInfo;
    }

    public class UserPost
    {
        public string SK = "playerName#LJMo_-#date#2022-02-11T06:27:34.499Z";
        public string playerID = "ljm9802mo@naver.com";
        public UserInfo playerInfo;
        public string PK = "PLAYER";
    }

    private static bool check = false;

    public class Users
    {
        public List<UserInfo> users = new List<UserInfo>();
    }

    private static cshEventManager instance = null;
    public static cshEventManager Instance
    {
        get { return instance; }
    }

    // DELEGATE FOR EVENT
    public delegate void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null);

    
    private Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        // make instance unique
        instance = this;

        // don't destroy 
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Users user_arr = new Users();
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
    public static void StopListening(string eventName, UnityAction listener)
    {
        if (instance == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
        if (eventName == "GameOver")
        {
            check = true;
        }
    }

    private void Update()
    {
        if (check)
        {
            MakeUserInfo();
        }
    }

    public void MakeUserInfo() 
    {
        UserInfo user_tmp = new UserInfo
        {
            EmailInfo = Email,
            GameScoreInfo = GameScore,
            GameTimeInfo = GameTime,
            GameDateTimeInfo = GameDateTime
        };

        UserPost user = new UserPost 
        {
            playerInfo = user_tmp
        };

        string json = JsonUtility.ToJson(user);
        Debug.Log(json);

        StartCoroutine(Upload(
            "https://kv2s3duleh.execute-api.ap-northeast-2.amazonaws.com/default/lungrow-minecraft-create-data"
            , json));

        check = false;
    }

    IEnumerator Upload(string URL, string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(URL, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Count-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}
