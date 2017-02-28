using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsBehaviors : MonoBehaviour {
    public void SetAudioVolume(float val)
    {
        GameController.GameControllerInstance.SetVolume(val);
    }

    public void MuteVolume(bool val)
    {
        GameController.GameControllerInstance.MuteVolume(val);
    }
}
