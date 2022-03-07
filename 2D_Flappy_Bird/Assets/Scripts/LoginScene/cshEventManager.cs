using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System;
using System.Text;

public enum EVENT_TYPE
{
    LOGIN_SUCCESS,
    LEVEL_SELECT,
    GAME_START,
    CHECK_INHALE_SPD,
    CHECK_EXHALE_SPD,
    SCORE_CHANGE,
    GAME_RESULT,
    GAME_OVER,
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

    public static bool check = false;

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

    private static Dictionary<EVENT_TYPE, List<OnEvent>> Listeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();

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

    public void AddListener(EVENT_TYPE Event_Type, OnEvent Listener)
    {
        // 이 이벤트를 수신할 리스너의 리스트
        List<OnEvent> ListenList = null;

        // 이벤트 형식 키가 존재하는지 검사한다. 존재하면 이를 리스트에 추가한다.
        if (Listeners.TryGetValue(Event_Type, out ListenList))
        {
            // 리스트가 존재하면 새 항목을 추가한다.
            ListenList.Add(Listener);
            Debug.Log(Event_Type);
            return;
        }

        // 아니면 새로운 리스트를 생성한다.
        ListenList = new List<OnEvent>();
        ListenList.Add(Listener);
        Listeners.Add(Event_Type, ListenList); // 내부의 Listner list에 추가한다.
    }

    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        // Notifiy all listeners of an event

        // List of listeners for this event only
        List<OnEvent> ListenList = null;

        // If no event entry exists, then exit because there are no listeners
        if (!Listeners.TryGetValue(Event_Type, out ListenList))
        {
            return;
        }

        // Event entry exists. Now notify appropriate listeners send message via delegate
        for (int i = 0; i < ListenList.Count; i++)
        {
            // If object is not null, then 
            if (!ListenList[i].Equals(null))
            {
                ListenList[i](Event_Type, Sender, Param);
            }
        }
    }

    /*

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
    */

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
