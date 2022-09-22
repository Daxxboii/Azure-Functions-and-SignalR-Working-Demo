using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using TMPro;
using BestHTTP.JSON;
using MyBox;
using PlayFab.Json;

public class SignalRClient : MonoBehaviour 
{
    // SignalR variables
    private static Uri uri = new Uri("https://deployingsignalrserver.azurewebsites.net/Gamehub");

    public static SignalRClient instance;

    private HubConnection connection;

    public TextMeshProUGUI ConnectingStatus,InvokedText, PlayerDetails,ChannelName;
    public GameObject EventButton;


    public static string SignalRID;

    //  Use this for initialization
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        Connect();
    }



    // Connect to the SignalR server
    public async void Connect()
    {
       connection = new HubConnectionBuilder().WithUrl(uri).Build();

       await connection.StartAsync();
       ConnectingStatus.text = "Connected";

       ChannelName.gameObject.SetActive(true);
        EventButton.SetActive(true);

        connection.On<string>("Data", (data) =>
        {
            InvokedText.gameObject.SetActive(true);
            PlayerDetails.gameObject.SetActive(true);

            dynamic DeserializedData = JsonConvert.DeserializeObject(data);
            PlayerDetails.text = "UserName"+ DeserializedData.PlayerProfile.LinkedAccounts[0].Username;

            dynamic NestedEventData = JsonConvert.DeserializeObject(DeserializedData.PlayStreamEventEnvelope.EventData.ToString());
           // Debug.Log(NestedEventData.XP);
        });


        connection.On<string>("SignalRID", (data) =>
        {
            SignalRID = data;
            Debug.Log(data);
        });


        await connection.InvokeAsync<string>("AddToChannel", "Group", PlayFabmanager.PlayerUsername);
        await connection.InvokeAsync<string>("SendSignalRIDToClient");
    }

   

    private async void OnApplicationQuit()
    {
        await connection.InvokeAsync<string>("RemoveFromChannel", "Public");
        await connection.StopAsync();
    }
}
