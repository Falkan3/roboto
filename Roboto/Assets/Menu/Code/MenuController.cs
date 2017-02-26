using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadScene(string input)
    {
        GameController.GameControllerInstance.GM_LoadScene(input);
    }

    public void LoadLevel()
    {
        try
        {
            string loadfilepath = UnityEditor.EditorUtility.OpenFilePanel("Load a map", Application.dataPath + "/Levels", "xml");
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
