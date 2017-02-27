using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    //File panels
    /*
    ExtensionFilter[] extensionOpen = new[] {
    new ExtensionFilter("XML Files", "xml" ),
    new ExtensionFilter("All Files", "*" ),
    };
    */

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string input)
    {
        GameController.GameControllerInstance.GM_LoadScene(input);
    }

    public void LoadLevelClick()
    {
        GameController.GameControllerInstance.displayFileDialog("Choose a map to load");
        StartCoroutine(AwatFileDialogResult());
    }

    public IEnumerator AwatFileDialogResult()
    {
        yield return StartCoroutine(GameController.GameControllerInstance.DialogResult());

        if (GameController.GameControllerInstance.SelectedFile != null)
        {
            if (GameController.GameControllerInstance.SelectedFile != "CANCEL")
            {
                Debug.Log(GameController.GameControllerInstance.SelectedFile);
                LoadLevel(Application.persistentDataPath + "/" + GameController.GameControllerInstance.SelectedFile);
            }
            GameController.GameControllerInstance.resetDialogResult();
        }
        yield break;
    }

    public void LoadLevel(string loadfilepath)
    {
        try
        {
            if(string.IsNullOrEmpty(loadfilepath))
            {
                Debug.Log("Selected file is invalid: " + loadfilepath);
            }
            else
            {
                Debug.Log("Selected level: " + loadfilepath);
                GameController.GameControllerInstance.SelectedLevelPath = loadfilepath;
                GameController.GameControllerInstance.GM_LoadScene("Level");
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("Failure selecting level: " + ex);
        }
    }
}
