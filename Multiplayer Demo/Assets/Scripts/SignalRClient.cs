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

    public TextMeshProUGUI ConnectingStatus,ChannelName;
    public GameObject EventButton;


    public string SignalRID;

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


        //On Recieving Public Data
        connection.On<string>("Data", (data) =>
        {
            dynamic DeserializedData = JsonConvert.DeserializeObject(data);
            dynamic NestedEventData = JsonConvert.DeserializeObject(DeserializedData.PlayStreamEventEnvelope.EventData.ToString());

            Debug.Log(DeserializedData);
        });

        //On Recieving Private Data
        connection.On<string>("PrivateData", (data) =>
        {
            dynamic DeserializedData = JsonConvert.DeserializeObject(data);
            dynamic NestedEventData = JsonConvert.DeserializeObject(DeserializedData.PlayStreamEventEnvelope.EventData.ToString());

            Debug.Log(DeserializedData);
        });

        //On Recieving Group Data
        connection.On<string>("PrivateGroupData", (data) =>
        {
            dynamic DeserializedData = JsonConvert.DeserializeObject(data);
            dynamic NestedEventData = JsonConvert.DeserializeObject(DeserializedData.PlayStreamEventEnvelope.EventData.ToString());

            Debug.Log(DeserializedData);
        });




        //On Receiving SignalR ID
        connection.On<string>("SignalRID", (data) =>
        {
            SignalRID = data;
        });


        await connection.InvokeAsync<string>("AddToGroup", "Group");
        await connection.InvokeAsync<string>("SendSignalRIDToClient");
    }

   

    private async void OnApplicationQuit()
    {
        await connection.InvokeAsync<string>("RemoveFromGroup", "Group");
        await connection.StopAsync();
    }
}
