using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;


public class Events : MonoBehaviour
{

    [SerializeField, ConstantsSelection(typeof(EventType))] private string EventInvokeType = "Nothing";


    [SerializeField,ConstantsSelection(typeof(PresetLayers))] private string DataType = "Nothing";
    [SerializeField] private string DataValue;

    public void SendEvent(){
        if(EventInvokeType=="All")PlayFabmanager.instance.SendEventToAll(SignalRClient.instance.SignalRID,DataType,DataValue);
        else if (EventInvokeType == "Private") PlayFabmanager.instance.SendEventToPrivatePlayer(SignalRClient.instance.SignalRID, DataType, DataValue);
        else PlayFabmanager.instance.SendEventToGroup(SignalRClient.instance.SignalRID,"GroupName", DataType, DataValue);
    }
}
public class PresetLayers
{
    public const string XP = "XP";
    public const string RandomData1 = "RandomData1";
    public const string RandomData2 = "RandomData2";
    public const string RandomData3 = "RandomData3";
}

public class EventType
{
    public const string All = "All";
    public const string Private = "Private";
    public const string Group = "Group";
}


