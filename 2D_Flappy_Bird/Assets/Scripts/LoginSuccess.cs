using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LoginSuccess : MonoBehaviour
{
    [SerializeField] Button nextButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        nextButton.onClick.AddListener(()=>{
            Debug.Log("next button clicked");
            SceneManager.LoadScene(2);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
