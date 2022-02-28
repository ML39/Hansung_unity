using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System;
using System.Text;

public enum EVENT_TYPE
{
    LOGINSUCCESS,
    GAMESTART,
    GAMEOVER,
};


public class cshEventManager : MonoBehaviour
{
    public static string Email;
    public static int GameScore;
    public static int GameTime;
    public static string GameDateTime;

    [System.Serializable]
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
        public List<UserInfo> playerInfo;
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

    
    private Dictionary<EVENT_TYPE, UnityEvent> eventDictionary = new Dictionary<EVENT_TYPE, UnityEvent>();

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

    public static void StartListening(EVENT_TYPE event_type, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(event_type, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(event_type, thisEvent);
        }
    }
    public static void StopListening(EVENT_TYPE event_type, UnityAction listener)
    {
        if (instance == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(event_type, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(EVENT_TYPE event_type)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(event_type, out thisEvent))
        {
            thisEvent.Invoke();
        }
        if (event_type == EVENT_TYPE.GAMEOVER)
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

        List<UserInfo> user_list = new List<UserInfo>();
        user_list.Add(user_tmp);

        UserPost user = new UserPost 
        {
            playerInfo = user_list
        };

        string json = JsonUtility.ToJson(user);
        Debug.Log(json);

        StartCoroutine(Upload(
            "https://kv2s3duleh.execute-api.ap-northeast-2.amazonaws.com/default/lungrow-minecraft-create-data"
            , json));
        StartCoroutine(GetRequest("https://aeptktnm55.execute-api.ap-northeast-2.amazonaws.com/default/lungrow-minecraft-getInfo-ByPlayerName?playerName=LJMo_-"));

        check = false;
    }

    IEnumerator Upload(string URL, string json)
    {
        var request = new UnityWebRequest(URL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

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

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
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

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
    }
}
