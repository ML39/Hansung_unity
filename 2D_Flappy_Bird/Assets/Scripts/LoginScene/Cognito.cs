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
using UnityEngine.Events;



public class Cognito : MonoBehaviour
{
    //UI elements 
    public Button SignupButton;
    public Button SignInButton;
    public Button ForgotPasswordButton; //
    public Button ConfirmForgetPasswordButton; //
   // public InputField EmailField;
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
    bool signInSuccessful;

    void Start()
    {
        SignupButton.onClick.AddListener(on_signup_click);
        SignInButton.onClick.AddListener(on_signin_click);
        ForgotPasswordButton.onClick.AddListener(on_forgotpassword_click);
        
        signInSuccessful = false;
        Debug.Log("Cognito auth begin");
        StatusText.text = "Cognito Auth";

        cshEventManager.Instance.AddListener(EVENT_TYPE.LOGIN_SUCCESS, LogINEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (signInSuccessful)
        {
            cshEventManager.Instance.PostNotification(EVENT_TYPE.LOGIN_SUCCESS, this, signInSuccessful);
            animator.SetBool("activity", false);
           // SceneManager.LoadScene(1);
            SceneManager.LoadScene("ble_test_scence1");
        }
    }

    public void on_signup_click()
    {
        Debug.Log("sign up button clicked");
        _ = SignUpMethodAsync();
    }

    public void on_signin_click()
    {
        Debug.Log("sign in button clicked");

        //
        animator.SetBool("activity", true);
        //Handheld.StartActivityIndicator();
        _ = SignInUser();
    }

    public void on_forgotpassword_click()
    {
        Debug.Log("forgot password clicked.");
        SceneManager.LoadScene("ForgotScene");
    }
 
    //Method that creates a new Cognito user
    private async Task SignUpMethodAsync()
    {
        string userName = UsernameField.text;
        string password = PasswordField.text;
      //  string email = EmailField.text;
      
        // remove white spaces
        userName = userName.Trim();
        password = password.Trim();


        AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Region);

        SignUpRequest signUpRequest = new SignUpRequest()
        {
            ClientId = AppClientID,
            Username = userName,
            Password = password
        };

        // List<AttributeType> attributes = new List<AttributeType>()
        // {
        //     new AttributeType(){Name = "email", Value = email}
        // };

        // signUpRequest.UserAttributes = attributes;

        try
        {
            SignUpResponse request = await provider.SignUpAsync(signUpRequest);
            Debug.Log("Sign up success");
            StatusText.text = "CHECK YOUR REGISTERED E-MAIL TO CONFIRM SIGNUP.";
        }
        catch( Exception e) {
            {
                Debug.Log("Exception : " + e);
                StatusText.text = "Signup error : " + e;
                return;
            }
        }

    }

    //Method that signs in Cognito user 
    private async Task SignInUser()
    {
        string userName = UsernameField.text;
        string password = PasswordField.text;
        // remove white spaces
        userName = userName.Trim();
        password = password.Trim();

        AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Region);
        CognitoUserPool userPool = new CognitoUserPool(PoolID, AppClientID, provider);
        CognitoUser user = new CognitoUser(userName,AppClientID,userPool,provider);

        InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
        {
            Password = password
        };

        try
        {
            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);
            GetUserRequest getUserRequest = new GetUserRequest();
            getUserRequest.AccessToken = authResponse.AuthenticationResult.AccessToken;
            Debug.Log("User Access Token: " + getUserRequest.AccessToken);
            signInSuccessful = true;
           
        }
        catch(Exception e)
        {
            Debug.Log("Exception: " + e);
            StatusText.text = "Signin error : " + e;
            return;
        }
    }

    public void LogINEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        cshEventManager.Email = UsernameField.text;
        Debug.Log("LogIN" + ", Time : " + DateTime.Now);
    }
}