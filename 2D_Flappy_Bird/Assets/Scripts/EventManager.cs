using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GAME EVENT
public enum EVENT_TYPE 
{
    GAME_INIT,
    GAME_END,
    AMMO_CHANGE,
    DEAD,
    LOGIN_SUCCESS,
    REHAB_DATA_UPDATED,
};

public class EventManager : MonoBehaviour
{
    // public event  Action<string> OnUserLoginSuccessEvent; 
    // public event Action<float> OnRehabDataUpdatedEvent;

    // singleton
    private static EventManager instance = null;
    public static EventManager Instance 
    {
        get {return instance;}
    }

    // DELEGATE FOR EVENT
    public delegate void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null);
    
    // LISTENER OBJECTS LIST
    private Dictionary<EVENT_TYPE, List<OnEvent>> Listeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();

    private void Awake() {
        if(instance)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        // make instance unique
        instance = this;

        // don't destroy 
         DontDestroyOnLoad(this.gameObject);
    }


    // //post user login success event
    // public void OnUserLoginSuccess(String userName)
    // {
    //     OnUserLoginSuccessEvent?.Invoke(userName);
    // }

    // public void OnRehabDataUpdated(float data)
    // {
    //     OnRehabDataUpdatedEvent?.Invoke(data);
    // }

/// <summary>
/// add listener object to listener list
/// </summary>
/// <param name="EVENT_TYPE"> Events to listen for  </param>
/// <param name="Listener"> Object to listen for event </param>
public void AddListener (EVENT_TYPE Event_Type, OnEvent Listener)
{
    // 이 이벤트를 수신할 리스너의 리스트
    List<OnEvent> ListenList = null;

    // 이벤트 형식 키가 존재하는지 검사한다. 존재하면 이를 리스트에 추가한다.
    if(Listeners.TryGetValue(Event_Type, out ListenList))
    {
        // 리스트가 존재하면 새 항목을 추가한다.
        ListenList.Add(Listener);
            Debug.Log(Event_Type);
        return ;
    }

    // 아니면 새로운 리스트를 생성한다.
    ListenList = new List<OnEvent>();
    ListenList.Add(Listener);
    Listeners.Add(Event_Type, ListenList); // 내부의 Listner list에 추가한다.

}

/// <summary>
//  function to post an event to listeners
/// </summary>
/// <param name="Event_Type"> Event to invoke  </param>
/// <param name="Sender">Object to invoke event</param>
/// <param name="Param">Optional arguments </param>
public void PostNotification(EVENT_TYPE Event_Type,  Component Sender, object Param = null )
{
    // Notifiy all listeners of an event

    // List of listeners for this event only
    List<OnEvent> ListenList = null;

    // If no event entry exists, then exit because there are no listeners
    if(!Listeners.TryGetValue(Event_Type, out ListenList))
        return;
    
    // Event entry exists. Now notify appropriate listeners send message via delegate
    for(int i=0; i<ListenList.Count; i++)
    {
        // If object is not null, then 
        if(!ListenList[i].Equals(null))
            ListenList[i](Event_Type, Sender,Param);
    }
}
    
}
