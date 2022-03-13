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
    SCORE_PLUS,
    SCORE_MINUS,
    GAME_RESULT,
    GAME_OVER,
};


public class cshEventManager : MonoBehaviour
{
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

    }

}
