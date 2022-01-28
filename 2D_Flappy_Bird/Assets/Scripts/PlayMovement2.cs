using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayMovement2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    //public Rigidbody2D rb ;
    //public Animator animator;
    Vector2 movement;
    [SerializeField] Button rightButton;
   

    // //Android
    // AndroidJavaObject _pluginInstance;
    // [SerializeField] Text _text;
    // [SerializeField] Button _button;

    // void Awake() {
    //     var pluginClass = new AndroidJavaClass("com.example.unityplugin.UnityPlugin");
    //     _pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("instance");
    //     _text.text = _pluginInstance.Call<string>("getPackageName");  
    //     _button.onClick.AddListener(() =>
    //     {
    //         _pluginInstance.Call("unitySendMessage", gameObject.name, "CallByAndroid", "Hello Android Plugin!");
    //     });   
    // }

    void Awake() {
        rightButton.onClick.AddListener(()=>{
            Debug.Log("right button clicked");
            SceneManager.LoadScene(1);
        });
    }


    // Update is called once per frame
    void Update()
    {
        // Input
        // movement.x = Input.GetAxisRaw("Horizontal");
        // movement.y = Input.GetAxisRaw("Vertical");

        // Animator
        // animator.SetFloat("Horizontal", movement.x);
        // animator.SetFloat("Vertical", movement.y);
        // animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate() 
    {
        // Movement
       // rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // // Android Java object
    // void CallByAndroid(string message)
    // {
    //     _pluginInstance.Call("showToast", message);
    // }
}
