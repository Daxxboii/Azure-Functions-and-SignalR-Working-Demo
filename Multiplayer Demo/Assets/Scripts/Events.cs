using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;


public class Events : MonoBehaviour
{
    [SerializeField,ConstantsSelection(typeof(PresetLayers))] private string EventToTrigger = "Nothing";
    public void SendEvent(){
        PlayFabmanager.instance.TriggerCustomEvent(EventToTrigger);
    }
}
public class PresetLayers
{
    public const string XPGiven = "XPGiven";
    public const string RandomEventOne = "RandomEvent1";
    public const string RandomEventTwo = "RandomEvent2";
    public const string RandomEventThree = "RandomEvent3";
}


