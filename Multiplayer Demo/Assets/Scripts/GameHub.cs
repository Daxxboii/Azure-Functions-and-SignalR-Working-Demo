using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;


// A class for connection with server hub
public class GameHub : Hub
{   
    public SignalRClient signalRClient;
    public GameHub(ref SignalRClient signalRClient) : base("GameHub")
    {
        this.signalRClient = signalRClient;
        base.On("OpponentLeft", Left);
        base.On("RecieveUpdatedXP", RecieveUpdatedXP);
    }

    // Do some operations when opponent left the game
    private void Left(Hub hub, MethodCallMessage msg)
    {
        // Back to the first scene
        Debug.Log("Player Disconnected!");
        SceneManager.LoadScene("Connect");
    }

    private void RecieveUpdatedXP(Hub hub,MethodCallMessage msg)
    {
       Debug.Log("Name And Data Recieved");
    }
}