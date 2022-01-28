using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;
using Newtonsoft.Json;

public class CognitoForgotPassword : MonoBehaviour
{
    //UI elements 
   
    public Button ForgotPasswordButton; //
    public Button ConfirmForgetPasswordButton; //
  
    public InputField PasswordField;
    public InputField UsernameField;
    public InputField ConfirmationCodeField;
    public Text StatusText;
    public Animator animator;

    const string PoolID = "us-east-1_xijYk0NA2"; //insert your Cognito User Pool ID, found under General Settings
    const string AppClientID = "1rhtr77kdf7ikiqa7rjnco63v8"; //insert App client ID, found under App Client Settings
    static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.USEast1; //insert region user pool was created in, if it is different than US EAST 1

    // minecraft-user-V2
    // const string PoolID = "ap-northeast-2_sh3FxQRAi"; //insert your Cognito User Pool ID, found under General Settings
    // const string AppClientID = "578cdjh05e1e4pbpdus6hge193"; //insert App client ID, found under App Client Settings
    // static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.APNortheast2; //insert region user pool was created in, if it is different than US EAST 1
    bool passwordChangeSuccessful;


    void Start()
    {
        
        ForgotPasswordButton.onClick.AddListener(on_forgotpassword_click);
        ConfirmForgetPasswordButton.onClick.AddListener(on_confirmforgotpassword_click);
        passwordChangeSuccessful = false;
        Debug.Log("FORGOT PASSWORD BEGINS");
        StatusText.text = "FORGOT PASSWORD MENU"; 
    }

    // Update is called once per frame
    void Update()
    {
        if (passwordChangeSuccessful)
        {
            animator.SetBool("activity", false);
            SceneManager.LoadScene("CognitoTestScene"); // move to login scene
        }
           
    }
       
    public void on_forgotpassword_click()
    {
        Debug.Log("forgot password clicked.");
        //SceneManager.LoadScene("TestScene");
        _ = ForgotPasswordMethodAsync();
    }

    public void on_confirmforgotpassword_click()
    {
        Debug.Log("confirm forgot password clicked.");
        _ = ConfirmForgotPasswordMethodAsync();
    }

    //
    private async Task ForgotPasswordMethodAsync()
    {
        string userName = UsernameField.text;
        AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Region);

        ForgotPasswordRequest forgotPasswordRequest = new ForgotPasswordRequest()
        {
            ClientId = AppClientID,
            Username = userName
        };

        try
        {
            ForgotPasswordResponse request = await provider.ForgotPasswordAsync(forgotPasswordRequest);
            Debug.Log("Forgot password request success");
            Debug.Log(JsonConvert.SerializeObject(request));
            StatusText.text = "CHECK YOUR REGISTERED E-MAIL FOR CONFIRMATION CODE.";
        }
        catch( Exception e) {
            {
                Debug.Log("Exception : " + e);
                return;
            }
        }
    }

    // confirm forgot password
     private async Task ConfirmForgotPasswordMethodAsync()
    {
        string userName = UsernameField.text;
        string password = PasswordField.text;
     
        string confirmationCode = ConfirmationCodeField.text;

        AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Region);

        ConfirmForgotPasswordRequest confirmForgotPasswordRequest = new ConfirmForgotPasswordRequest()
        {
            ClientId = AppClientID,
            Username = userName,
            Password = password,
            ConfirmationCode = confirmationCode,
        };

        try
        {
            ConfirmForgotPasswordResponse request = await provider.ConfirmForgotPasswordAsync(confirmForgotPasswordRequest);
            Debug.Log("Confirm Forgot password request success");
           // Debug.Log(JsonConvert.SerializeObject(request));
            StatusText.text = "PASSWORD SUCCESSFULLY CHANGED !";
            // set
            passwordChangeSuccessful = true;
           
        }
        catch( Exception e) {
            {
                Debug.Log("Exception : " + e);
                return;
            }
        }
    }

 

}