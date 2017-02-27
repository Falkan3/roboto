using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertPanelBehaviors : MonoBehaviour {
    public void sendOK()
    {
        Debug.Log("Hit OK");
        GameController.GameControllerInstance.sendAlertResult("OK");
        //gameObject.SetActive(false);
    }

    public void sendCancel()
    {
        Debug.Log("Hit Cancel");
        GameController.GameControllerInstance.sendAlertResult("CANCEL");
        //gameObject.SetActive(false);
    }
}
