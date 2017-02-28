using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    //Regex
    public Regex RGX_number = new Regex("^[0-9]{1,}$");
    //Scene to load
    public bool firstBoot = true;
    private Screen_fade screenFade;
    private AudioSource audioSource;
    //Alert components
    private GameObject alertPanel;
    private Text alertTitle, alertBody;
    private GameObject alertButtonOK, alertButtonCancel;
    private string result;
    //FileDialog components
    private GameObject fileDialogPanel;
    private Text fileDialogTitle;
    private Dropdown fileDialogDropdown;
    private string selectedFile;
    //Audio components
    private float maxVolume;
    private GameObject settingsPanel;
    private Slider audioSlider;
    private Toggle audioToggle;

    private string selectedLevelPath;

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
                initInstance(instance, gB);
                
            }
            return instance;
        }
    }

    #region Properties
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

    public string SelectedLevelPath
    {
        get
        {
            return selectedLevelPath;
        }

        set
        {
            selectedLevelPath = value;
        }
    }

    public string Result
    {
        get
        {
            return result;
        }

        set
        {
            result = value;
        }
    }

    public string SelectedFile
    {
        get
        {
            return selectedFile;
        }

        set
        {
            selectedFile = value;
        }
    }

    public GameObject FileDialogPanel
    {
        get
        {
            return fileDialogPanel;
        }

        set
        {
            fileDialogPanel = value;
        }
    }

    public GameObject AlertPanel
    {
        get
        {
            return alertPanel;
        }

        set
        {
            alertPanel = value;
        }
    }

    public float MaxVolume
    {
        get
        {
            return maxVolume;
        }

        set
        {
            maxVolume = value;
        }
    }
    #endregion

    void Start()
    {
        initComponents();
        Cursor.SetCursor(Resources.Load("cursor") as Texture2D, Vector2.zero, CursorMode.Auto);
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        if(Input.GetButtonDown("Settings"))
        {
            toggleSettingsPanel();
        }
    }

    /// <summary>
    /// Function section
    /// </summary>

    #region inits
    void initComponents()
    {
        instance = this;
        ScreenFade = GetComponent<Screen_fade>();
        AudioSource = GetComponent<AudioSource>();

        initAlerts();
        initFileDialogs();
        initAudioSettings();
        loadPlayerPrefs();

        if (firstBoot == true)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
            FadeAudio_f(fadeTime, Fade.In);
        }
    }

    void loadPlayerPrefs()
    {
        //Volume
        if (PlayerPrefs.HasKey("MaxVolume"))
            instance.MaxVolume = PlayerPrefs.GetFloat("MaxVolume");
        else
            instance.MaxVolume = 1f;
        instance.audioSlider.value = instance.maxVolume;
        //Mute
        if (PlayerPrefs.HasKey("Mute"))
        {
            bool temp = bool.Parse(PlayerPrefs.GetString("Mute"));
            instance.MuteVolume(temp);
            instance.audioToggle.isOn = temp;
        }    
        else
            instance.audioToggle.isOn = true;


    }

    void initAlerts()
    {
        AlertPanel = GameObject.Find("AlertPanel");
        alertTitle = GameObject.Find("AlertTitle").GetComponent<Text>();
        alertBody = GameObject.Find("AlertBody").GetComponent<Text>();
        alertButtonOK = GameObject.Find("AlertButtonOK");
        alertButtonCancel = GameObject.Find("AlertButtonCancel");
        AlertPanel.SetActive(false);
    }

    void initFileDialogs()
    {
        FileDialogPanel = GameObject.Find("FileDialogPanel");
        fileDialogTitle = GameObject.Find("FileDialogTitle").GetComponent<Text>();
        fileDialogDropdown = GameObject.Find("FileDialogDropdown").GetComponent<Dropdown>();

        /*
        fileDialogDropdown.onValueChanged.AddListener(delegate
        {
            fileDialogPanel.GetComponent<AlertPanelBehaviors>().fileDialogSendFile(fileDialogDropdown);
        });
        */

        FileDialogPanel.SetActive(false);
    }

    void initAudioSettings()
    {
        settingsPanel = GameObject.Find("SettingsPanel");
        audioSlider = GameObject.Find("AudioSlider").GetComponent<Slider>();
        audioToggle = GameObject.Find("AudioToggle").GetComponent<Toggle>();
        settingsPanel.SetActive(false);
    }

    static void initInstance(GameController instance, GameObject gB)
    {
        instance = gB.AddComponent<GameController>();
        instance.screenFade = gB.AddComponent<Screen_fade>();
        instance.screenFade.delay = 4;
        instance.screenFade.fadeTexture = Resources.Load("Textures/Black") as Texture2D;
        instance.AudioSource = gB.AddComponent<AudioSource>();
        instance.AudioSource.loop = true;
        instance.firstBoot = false;
        Canvas canvas = new Canvas();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvas.transform.parent = instance.transform;
        //Panel
        instance.AlertPanel = new GameObject("AlertPanel");
        RectTransform panelRectTransform = instance.AlertPanel.AddComponent<RectTransform>();
        panelRectTransform.anchoredPosition = new Vector2(0, 0);
        panelRectTransform.sizeDelta = new Vector2(400, 225);
        instance.AlertPanel.AddComponent<Image>().sprite = Resources.Load("scifi_menu") as Sprite;
        instance.AlertPanel.transform.parent = canvas.transform;
        //Title
        instance.alertTitle = new GameObject("AlertTitle").AddComponent<Text>();
        instance.alertTitle.resizeTextForBestFit = true;
        instance.alertTitle.font = Resources.Load("airstrikehalf") as Font;
        instance.alertTitle.alignment = TextAnchor.MiddleCenter;
        //Body
        instance.alertBody = new GameObject("AlertBody").AddComponent<Text>();
        instance.alertBody.resizeTextForBestFit = true;
        instance.alertBody.font = Resources.Load("nasalization-rg") as Font;
        instance.alertBody.alignment = TextAnchor.MiddleCenter;
        instance.alertBody.resizeTextMaxSize = 20;
        //Buttons
        instance.alertButtonOK = new GameObject("AlertButtonOK");
        instance.alertButtonOK.AddComponent<Image>().sprite = Resources.Load("scifi_button_inactive") as Sprite;
        instance.alertButtonCancel = new GameObject("AlertButtonCancel");
        instance.alertButtonCancel.AddComponent<Image>().sprite = Resources.Load("scifi_button_inactive") as Sprite;
        //Behavior
        instance.AlertPanel.AddComponent<AlertPanelBehaviors>();
        instance.alertButtonOK.GetComponent<Button>().onClick.AddListener(delegate { instance.AlertPanel.GetComponent<AlertPanelBehaviors>().sendOK(); });
        instance.alertButtonCancel.GetComponent<Button>().onClick.AddListener(delegate { instance.AlertPanel.GetComponent<AlertPanelBehaviors>().sendCancel(); });

        instance.AlertPanel.SetActive(false);
    } 
    #endregion

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
        GameObject.Find("DialogCanvas").GetComponent<Canvas>().worldCamera = Camera.main;
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

    public void SetVolume(float val)
    {
        instance.audioSource.volume = val;
        instance.maxVolume = val;
        PlayerPrefs.SetFloat("MaxVolume", val);
    }

    public void MuteVolume(bool val)
    {
        instance.AudioSource.mute = val;
        instance.audioToggle.isOn = val;
        PlayerPrefs.SetString("Mute", val.ToString());
    }

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
        float start = fadeType == Fade.In ? AudioSource.volume : instance.maxVolume;
        float end = fadeType == Fade.In ? instance.maxVolume : 0.0f;
        float i = 0.0f;
        float step = 1f / timer;

        while (i <= instance.maxVolume)
        {
            i += step * Time.deltaTime;
            AudioSource.volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
    }

    #region alert
    public void displayAlert(string alertTitle, string errorMsg, bool options)
    {
        instance.alertTitle.text = alertTitle;
        instance.alertBody.text = errorMsg;
        if (options)
            instance.alertButtonCancel.SetActive(true);
        else
            instance.alertButtonCancel.SetActive(false);
        instance.AlertPanel.SetActive(true);
    }

    public void sendAlertResult(string result)
    {
        instance.Result = result;
        instance.AlertPanel.SetActive(false);
    }

    public void resetAlertResult()
    {
        instance.Result = null;
    }

    public IEnumerator AlertResult()
    {
        instance.Result = null;
        while (instance.Result == null)
        {
            yield return null; // wait until next frame
        }
        yield break;
    }
    #endregion

    #region fileDialog
    public void displayFileDialog(string dialogTitle)
    {
        instance.fileDialogDropdown.ClearOptions();

        //Create placeholder item
        Dropdown.OptionData placeholder = new Dropdown.OptionData();
        placeholder.text = "Select...";

        //Read files from filepanel
        FilePanel fp = new FilePanel();
        instance.fileDialogTitle.text = dialogTitle;
        List<Dropdown.OptionData> items = new List<Dropdown.OptionData>();
        items.Add(placeholder);
        FileInfo[] filenames = fp.ReadFilesInFolder("");
        foreach(FileInfo x in filenames)
        {
            Dropdown.OptionData dropdowndata = new Dropdown.OptionData();
            dropdowndata.text = x.Name.ToString();
            items.Add(dropdowndata);
        }
        instance.fileDialogDropdown.AddOptions(items);
        
        instance.FileDialogPanel.SetActive(true);
    }

    public void sendDialogResult(string result)
    {
        instance.SelectedFile = result;
    }

    public void resetDialogResult()
    {
        instance.SelectedFile = null;
    }

    public IEnumerator DialogResult()
    {
        instance.SelectedFile = null;
        while (instance.SelectedFile == null)
        {
            yield return null; // wait until next frame
        }
        instance.FileDialogPanel.SetActive(false);
        yield break;
    }
    #endregion

    #region settings
    public void toggleSettingsPanel()
    {
        if(settingsPanel.activeSelf)
            settingsPanel.SetActive(false);
        else
            settingsPanel.SetActive(true);
    }
    #endregion
}
