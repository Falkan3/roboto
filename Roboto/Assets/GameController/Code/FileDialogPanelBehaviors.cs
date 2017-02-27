using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileDialogPanelBehaviors : MonoBehaviour {
    private UnityEngine.UI.Dropdown dropdown;

    void Start()
    {
        dropdown = GameObject.Find("FileDialogDropdown").GetComponent<UnityEngine.UI.Dropdown>();
    }

    public void fileDialogCancel()
    {
        Debug.Log("Hit Cancel");
        GameController.GameControllerInstance.sendDialogResult("CANCEL");
        //gameObject.SetActive(false);
    }

    public void fileDialogSendFile(int index)
    {
        Debug.Log(dropdown.value);
        if(index>0)
        {
            GameController.GameControllerInstance.sendDialogResult(dropdown.options[index].text);
        }
        dropdown.value = 0;
        dropdown.Select();
        dropdown.RefreshShownValue();
        //gameObject.SetActive(false);
    }
}
