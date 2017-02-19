using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameController : MonoBehaviour {
    //Regex
    static Regex RGX_number = new Regex("^[0-9]$");
    //Scene to load
    static bool firstBoot = true;

    void Start()
    {
        if(firstBoot == true)
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public static void GM_LoadScene(string input)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        if (RGX_number.IsMatch(input))
        {
            int index;
            int.TryParse(input, out index);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(input);
        }
    }
}
