using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInfo : MonoBehaviour
{
    void Start()
    {
        UnityEngine.UI.Text key1 = GameObject.Find("Text_Settings").GetComponent<UnityEngine.UI.Text>();
        UnityEngine.UI.Text key2 = GameObject.Find("Text_Action").GetComponent<UnityEngine.UI.Text>();
        UnityEngine.UI.Text key3 = GameObject.Find("Text_Movement").GetComponent<UnityEngine.UI.Text>();
        key1.text = "F1";
        key2.text = "Space";
        key3.text = "WASD";
    }
}