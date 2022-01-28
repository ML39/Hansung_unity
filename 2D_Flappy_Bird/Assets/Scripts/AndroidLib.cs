using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class AndroidLib : MonoBehaviour
{
    //Android
    AndroidJavaObject _pluginInstance;
    [SerializeField] Text _text;
    [SerializeField] Button _button;

    void Awake() {
        var pluginClass = new AndroidJavaClass("com.example.unityplugin.UnityPlugin");
        _pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("instance");
        _text.text = _pluginInstance.Call<string>("getPackageName");  
        _button.onClick.AddListener(() =>
        {
            _pluginInstance.Call("unitySendMessage", gameObject.name, "CallByAndroid", "Hello Android Plugin!");
        });   
    }

    // Android Java object
    void CallByAndroid(string message)
    {
        _pluginInstance.Call("showToast", message);
    }
}
