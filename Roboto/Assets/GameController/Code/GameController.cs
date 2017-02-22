using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameController : MonoBehaviour {
    //Regex
    public Regex RGX_number = new Regex("^[0-9]{1,}$");
    //Scene to load
    public bool firstBoot = true;
    private Screen_fade screenFade;
    private AudioSource audioSource;

    //Fade
    //Audio
    public float fadeTime = 0.5f;
    public enum Fade { In, Out }

    private static GameController instance;
    public static GameController GameControllerInstance
    {
        get
        {
            if (instance == null)
            {
                var gB = new GameObject("GameController");
                instance = gB.AddComponent<GameController>();
                instance.screenFade = gB.AddComponent<Screen_fade>();
                instance.screenFade.delay = 4;
                instance.screenFade.fadeTexture = Resources.Load("Textures/Black") as Texture2D;
                instance.AudioSource = gB.AddComponent<AudioSource>();
                instance.AudioSource.loop = true;
                instance.firstBoot = false;
            }
            return instance;
        }
    }

    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }

        set
        {
            audioSource = value;
        }
    }

    public Screen_fade ScreenFade
    {
        get
        {
            return screenFade;
        }

        set
        {
            screenFade = value;
        }
    }

    void Start()
    {
        instance = this;
        ScreenFade = GetComponent<Screen_fade>();
        AudioSource = GetComponent<AudioSource>();
        if(firstBoot == true)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
            FadeAudio_f(fadeTime, Fade.In);
        }
            
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void GM_LoadScene(string input)
    {
        if (GameControllerInstance.RGX_number.IsMatch(input))
        {
            int index;
            int.TryParse(input, out index);
            //GameControllerInstance.StartCoroutine(GameControllerInstance.FadeAudio_LoadScene(GameControllerInstance.fadeTime, Fade.Out, null, index));

            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
        }
        else
        {
            //GameControllerInstance.StartCoroutine(GameControllerInstance.FadeAudio_LoadScene(GameControllerInstance.fadeTime, Fade.Out, input, -1));

            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(input);
        }
        FadeAudio_f(fadeTime, Fade.Out);
    }

    //Helpers

    /*
    IEnumerator FadeAudio_LoadScene(float timer, Fade fadeType, string inputString, int inputInt)
    {
        float start = fadeType == Fade.In ? 0.0f : 1.0f;
        float end = fadeType == Fade.In ? 1.0f : 0.0f;
        float i = 0.0f;
        float step = 1.0f / timer;

        while (i <= 1.0)
        {
            i += step * Time.deltaTime;
            AudioSource.volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }

        if(string.IsNullOrEmpty(inputString))
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(inputInt);
        else
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(inputString);

        GameControllerInstance.ScreenFade.FadeInit(Screen_fade.Fade.Out, 1f);
    }
    */

    public void FadeAudio_f(float timer, Fade fadeType)
    {
        GameControllerInstance.StartCoroutine(FadeAudio(timer, fadeType));
    }

    public void FadeAudio_break()
    {
        StopCoroutine("FadeAudio");
    }

    IEnumerator FadeAudio(float timer, Fade fadeType)
    {
        float start = fadeType == Fade.In ? AudioSource.volume : 1.0f;
        float end = fadeType == Fade.In ? 1.0f : 0.0f;
        float i = 0.0f;
        float step = 1.0f / timer;

        while (i <= 1.0)
        {
            i += step * Time.deltaTime;
            AudioSource.volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
    }
}
