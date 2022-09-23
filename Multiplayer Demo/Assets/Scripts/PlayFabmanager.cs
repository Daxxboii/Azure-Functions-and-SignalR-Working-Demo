using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabmanager : MonoBehaviour
{
    public static PlayFabmanager instance;

    [SerializeField] GameObject signUpTab, logInTab, startPanel;
    [Header("Sign Up")]
    public TextMeshProUGUI username;
    public TextMeshProUGUI userEmail;
    public TextMeshProUGUI userPassword;
    public TextMeshProUGUI errorSignUp;

    [Header("Log In")]
    public TextMeshProUGUI userEmailLogin;
    public TextMeshProUGUI userPasswordLogin;
    public TextMeshProUGUI errorLogin;

    string encryptedPassword;

    public static string PlayerUsername;

    private void Awake()
    {
        if (instance == null) instance = this;
        DontDestroyOnLoad(this);
    }

    #region Signup and Login

    public void SignUpTab()
    {
        signUpTab.SetActive(true);
        logInTab.SetActive(false);
        startPanel.SetActive(false);
    }

    public void LoginTab()
    {
        signUpTab.SetActive(false);
        logInTab.SetActive(true);
        startPanel.SetActive(false);
        errorSignUp.text = " ";
        errorLogin.text = " ";
    }

    public void MenuTab()
    {
        signUpTab.SetActive(false);
        logInTab.SetActive(false);
        startPanel.SetActive(true);
        errorSignUp.text = " ";
        errorLogin.text = " ";
    }

    string Encrypt(string StringToEncrypt)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(StringToEncrypt);

        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();

        foreach(byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        return s.ToString();
    }

    public void SignUp()
    {
       // Debug.Log(username.text);
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail.text, Password = Encrypt(userPassword.text),Username=username.text.Substring(0,username.text.Length-1) };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterFailure);
    }

    void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        errorSignUp.text = " ";
        PlayerUsername = result.Username;
        SceneManager.LoadScene("Connect");
    }

    void RegisterFailure(PlayFabError error)
    {
        errorSignUp.text = "Error Signing Up ";
        Debug.Log(error);
    }

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmailLogin.text, Password = Encrypt(userPasswordLogin.text) };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, loginFailure);
    }
    void loginFailure(PlayFabError error)
    {
        errorLogin.text = "Error Logging in ";
        Debug.Log(error);
    }

    void LoginSuccess(LoginResult login)
    {
        errorLogin.text = " ";
        PlayerUsername = login.InfoResultPayload.PlayerProfile.DisplayName;

        SceneManager.LoadScene("Connect");
    }
    #endregion

    #region Save And Load
    public void SaveData()
    {
        var request = new UpdateUserDataRequest
        {
           // Data = Any Custom Dictionary
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {

    }

    void OnError(PlayFabError error)
    {

    }

    public void LoadData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
    }

    void OnDataRecieved(GetUserDataResult data)
    {
        if (data.Data != null)
        {

        }
        else
        {

        }

    }

    #endregion

    #region Custom Events to Trigger Playfab Functions

    public void SendEventToAll(string PlayerSignalRID,string DataType,string DataValue)
    {
        PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest {
            EventName = "SendToAll",
             Body = new Dictionary<string, object> {
            { DataType,DataValue},
            { "SignalRID", PlayerSignalRID}}}

        ,result=> Debug.Log("Event Called"),error=> Debug.LogError(error.GenerateErrorReport()));
     
    }

    public void SendEventToGroup(string PlayerSignalRID,string GroupName,string DataType, string DataValue)
    {
        PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest
        {
            EventName = "SendToGroup",
            Body = new Dictionary<string, object> {
            { DataType, DataValue },
            { "SignalRID", PlayerSignalRID},
            { "GroupName", GroupName} }
        }
        , result => Debug.Log("Event Called"), error => Debug.LogError(error.GenerateErrorReport()));

    }
    public void SendEventToPrivatePlayer(string PlayerSignalRID,string DataType, string DataValue)
    {
        PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest
        {
            EventName = "SendToPlayer",
            Body = new Dictionary<string, object> {
            { DataType, DataValue },
            { "SignalRID",PlayerSignalRID}}
        }
        , result => Debug.Log("Event Called"), error => Debug.LogError(error.GenerateErrorReport()));

    }


    #endregion

}
