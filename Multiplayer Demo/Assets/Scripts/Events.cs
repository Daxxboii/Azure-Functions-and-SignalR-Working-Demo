using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Events : MonoBehaviour
{
    public void SendEvent(){
        PlayFabmanager.instance.TriggerCustomEvent();
    }
}
