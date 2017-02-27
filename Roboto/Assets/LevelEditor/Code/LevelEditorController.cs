using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Collections;

public class LevelEditorController : MonoBehaviour {
    //General
    private LevelLoader levelLoader;

    //LevelSettings
    private string mapname="New Map";
    private int levelWidth = 16;
    private int levelHeight = 16;
    //
    private int tileLayer = 0;
    private short tileRotation = 0;
    List<GameObject> tileButtons = new List<GameObject>();
    List<SetActiveTile> tileScriptList = new List<SetActiveTile>();
    private int prefTileListIndex = 0;
    //active tile
    private int activeTileIndex = -1;
    private int activeObjectIndex = -1;
    private GameObject activeObject; //active tile to place
    private EditorTile activeObjectProperties;

    //EditorUI
    private Vector2 ray;
        //TilePickerTexture
        public GameObject editorTexture;
            private SpriteRenderer editorTextureRenderer;
            private Sprite editorDefaultSprite;
            private Color editorDefaultColor;
            //LevelBoundsTexture
            public GameObject editorBoundsTexture;
            private Vector2 editorBoundsTextureScale;
    private Bounds mainCameraBounds;
    private GameObject navigationPanel;
    private GameObject optionsPanel;
    private GameObject tilePanel;
    //inputs
    private InputField Input_LevelName;
    private InputField Input_LevelWidth;
    private InputField Input_LevelHeight;
    private InputField Input_Layer;
    //The text that shows tile coordinates
    private Text coordinatesText;
    //The text that shows tile layer
    private Text layerText;
    //Regex for only numbers
    Regex numberRGX = new Regex("^[0-9]{1,}$");
    Regex nonSpecialCharRGX = new Regex("^[a-zA-Z0-9]{1,20}$");
    //Click counter to delay spam
    private float clickCounter = 0.0f;
    private float clickCounterDelay = 0.02f;

    //Main
    //Tile object in prefab panel
    public GameObject menutile;
    int editorTileCount;
    //List of tiles in level, it stores the tile class object and corresponding gameobject
    public List<CombinedTile> levelTiles = new List<CombinedTile>();
    //the list of available prefabs is contained in LevelLoader

    //File panels
    /*
    ExtensionFilter[] extensionOpen = new[] {
    new ExtensionFilter("XML Files", "xml" ),
    new ExtensionFilter("All Files", "*" ),
    };
    ExtensionFilter[] extenstionSave = new[] {
    new ExtensionFilter("Binary", "bin"),
    new ExtensionFilter("Text", "txt"),
};
*/


    // Use this for initialization
    void Start () {
        //Get reference of level loader
        levelLoader = GetComponent<LevelLoader>();
        //Get reference of ui objects
        initializeUI();
        //Set texture for level bounds
        resizeLevelBoundaries();
	}
	
	// Update is called once per frame
	void Update () {
        moveEditorTexture();

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            if (clickCounter >= clickCounterDelay)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    PlaceTile();
                }

                clickCounter = 0;
            }
        }
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            if (clickCounter >= clickCounterDelay)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (activeTileIndex == -1)
                        RemoveTile();
                    else
                    {
                        ResetActiveTile();
                        clickCounter = -0.5f;
                    }
                }    
            }
        }
        if (Input.GetMouseButtonDown(2)) Debug.Log("Pressed middle click.");

        if(clickCounter <= clickCounterDelay)
            clickCounter += Time.deltaTime;
    }

    void moveEditorTexture()
    {
        ray.x = Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
        ray.y = Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        editorTexture.transform.position = new Vector2(ray.x, ray.y);
        coordinatesText.text = "Coordinates: X = " + ray.x + " Y = " + ray.y;
    }

    void resizeLevelBoundaries()
    {
        editorBoundsTextureScale = editorBoundsTexture.transform.localScale;
        editorBoundsTextureScale.x = levelWidth/2;
        editorBoundsTextureScale.y = levelHeight/2;
        editorBoundsTexture.transform.localScale = editorBoundsTextureScale;
        editorBoundsTextureScale.x = levelWidth * 0.5f - 0.5f;
        editorBoundsTextureScale.y = levelHeight * 0.5f - 0.5f;
        editorBoundsTexture.transform.position = new Vector2(0.5f, 0.5f);
    }

    void initializeUI()
    {
        navigationPanel = GameObject.Find("NavigationPanel");
        optionsPanel = GameObject.Find("OptionsPanel");
        tilePanel = GameObject.Find("TilePanel");
        optionsPanel.SetActive(true);
        mainCameraBounds = Camera.main.GetComponent<Aspect>().OrthographicBounds();
        coordinatesText = GameObject.Find("CoordinatesText").GetComponent<Text>();
        layerText = GameObject.Find("LayerText").GetComponent<Text>();

        editorTextureRenderer = editorTexture.GetComponent<SpriteRenderer>();
        editorDefaultSprite = editorTextureRenderer.sprite;
        editorDefaultColor = editorTextureRenderer.color;

        Input_LevelName = GameObject.Find("InputMapName").GetComponent<InputField>();
        Input_LevelWidth = GameObject.Find("InputLevelWidth").GetComponent<InputField>();
        Input_LevelHeight = GameObject.Find("InputLevelHeight").GetComponent<InputField>();
        Input_Layer = GameObject.Find("InputTileLayer").GetComponent<InputField>();

        ApplySettings();

        initializeTileSelector();
    }

    void initializeTileSelector()
    {
        editorTileCount = Mathf.FloorToInt(Camera.main.pixelWidth / (48f+24f));
        Vector2 pos;
        GameObject newTile;
        for (int i = 0; i < editorTileCount; i++)
        {
            newTile = Instantiate(menutile, new Vector3(0, 0), Quaternion.identity) as GameObject;
            pos = new Vector2(tilePanel.transform.position.x + (i * (48+4) + 24), 0);
            newTile.transform.SetParent(tilePanel.transform);
            newTile.transform.localScale = new Vector3(1, 1, 1);
            newTile.transform.localPosition = pos;
            tileButtons.Add(newTile);
        }

        //Populate setactivetile scripts of each tile button object
        tileScriptList.Clear();
        for (int i = 0; i < tileButtons.Count; i++)
        {
            tileScriptList.Add(tileButtons[i].GetComponent<SetActiveTile>());
        }
        prefTileListIndex = 0;

        populateTileSelector();
    }

    void populateTileSelector()
    {
        for (int i = 0; i < tileButtons.Count; i++)
        {
            tileScriptList[i].Index = prefTileListIndex + i;

            if (prefTileListIndex + i < levelLoader.prefabTilesList.Count)
                tileScriptList[i].changeSelf(levelLoader.prefabTilesList[i], levelLoader.prefabTilesList[i].GetComponent<TileInitiator>().tile.Index);
            else
                tileScriptList[i].resetSelf();
        }
        Debug.Log("index in list:" + prefTileListIndex + " number of prefabs found: " + levelLoader.prefabTilesList.Count);
    }

    public void changeListIndex()
    {
        prefTileListIndex -= (int)(Input.GetAxis("Mouse ScrollWheel") * 10);
        if (prefTileListIndex > levelLoader.prefabTilesList.Count)
            prefTileListIndex = levelLoader.prefabTilesList.Count;
        else if (prefTileListIndex < 0)
            prefTileListIndex = 0;
        populateTileSelector();

        Debug.Log("index in list:" + prefTileListIndex);
    }

    //Editor Methods ------------------------------------------------------------------>
    public void toggleOptionsPanel()
    {
        if(optionsPanel.activeSelf == false)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            optionsPanel.SetActive(false);
        }
    }

    public void SetMapName(string name)
    {
        if(nonSpecialCharRGX.IsMatch(name))
            mapname = name;
    }

    public void ChangeLevelWidth(string width)
    {
        if (numberRGX.IsMatch(width))
        {
            int.TryParse(width, out levelWidth);
            ApplySettings();
        }
    }

    public void ChangeLevelHeight(string height)
    {
        if (numberRGX.IsMatch(height))
        {
            int.TryParse(height, out levelHeight);
            ApplySettings();
        }
    }

    public void ChangeLayer(string layer)
    {
        if (numberRGX.IsMatch(layer))
        {
            int.TryParse(layer, out tileLayer);
            layerText.text = "Layer: " + tileLayer;
            //CombinedTile[] temp = levelTiles.Where(obj => obj.Tile.X == (int)ray.x && obj.Tile.Y == (int)ray.y && obj.Tile.Layer == tileLayer);
            foreach(var obj in levelTiles)
            {
                SpriteRenderer sr = obj.GameObject.GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            }
            var temp = from obj in levelTiles
                                      where obj.Tile.Layer != tileLayer
                                      select obj;
            foreach (var obj in temp)
            {
                SpriteRenderer sr = obj.GameObject.GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
            }

            ApplySettings();
        }
    }

    public void ApplySettings()
    {
        resizeLevelBoundaries();
        Input_LevelName.text = mapname;
        Input_LevelWidth.text = levelWidth.ToString();
        Input_LevelHeight.text = levelHeight.ToString();
        Input_Layer.text = tileLayer.ToString();
    }

    public void ExitToMainMenu()
    {
        GameController.GameControllerInstance.GM_LoadScene("1");
    }

    public void ChangeActiveTile(int ind, GameObject obj, int tileIndex)
    {
        activeTileIndex = ind;
        activeObject = obj;
        activeObjectIndex = tileIndex;
        activeObjectProperties = obj.GetComponent<TileInitiator>().tile;
        SpriteRenderer newSprite = obj.GetComponent<SpriteRenderer>();
        Debug.Log("selected tile with index: " + ind + " obj: " + obj.name + " sprite renderer: " + newSprite.sprite + " tile index: " + activeObjectIndex);
        editorTexture.transform.localScale = new Vector3(1, 1, 1);
        editorTextureRenderer.sprite = newSprite.sprite;
        editorTextureRenderer.color = newSprite.color;

        tileRotation = 0;
    }

    public void ResetActiveTile()
    {
        if(activeTileIndex != -1)
        {
            activeTileIndex = -1;
            activeObject = null;
            activeObjectIndex = -1;
            editorTexture.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            editorTextureRenderer.sprite = editorDefaultSprite;
            editorTextureRenderer.color = editorDefaultColor;

            Debug.Log("Resetting editor tile to: " + editorDefaultSprite + " " + editorDefaultColor);
        }
    }

    public void RemoveTile()
    {
        if(activeTileIndex == -1)
        {
            int temp = levelTiles.FindIndex(obj => obj.Tile.X == (int)ray.x && obj.Tile.Y == (int)ray.y && obj.Tile.Layer == tileLayer);
            if (temp != -1)
            {
                Debug.Log("Tile found. Removing...");

                Destroy(levelTiles[temp].GameObject);
                levelTiles.RemoveAt(temp);

                Debug.Log("Tile removed at " + ray.x + " " + ray.y + " layer: " + tileLayer);
            }
            else
            {
                Debug.Log("Tile not found in given layer " + tileLayer);
            }
        }
    }

    public void PlaceTile()
    {
        if (activeTileIndex != -1 && activeObject != null && activeObjectIndex != -1)
        {
            if (editorTexture.transform.position.x >= Mathf.Ceil(levelWidth / -2 + 0.5f) &&
                editorTexture.transform.position.x <= Mathf.Ceil(levelWidth / 2 - 0.5f) &&
                editorTexture.transform.position.y <= Mathf.Ceil(levelHeight / 2 - 0.5f) &&
                editorTexture.transform.position.y >= Mathf.Ceil(levelHeight / -2 + 0.5f))
            {
                CombinedTile temp = levelTiles.Where(obj => obj.Tile.X == (int)ray.x && obj.Tile.Y == (int)ray.y && obj.Tile.Layer == tileLayer).SingleOrDefault<CombinedTile>();
                if (temp == null)
                {
                    temp = new CombinedTile(null, null);
                    temp.Tile = new Tile(activeObjectIndex, activeObject, activeObject.name, tileRotation, tileLayer, (int)ray.x, (int)ray.y, activeObjectProperties.Dimensions, activeObjectProperties.Category);
                    temp.GameObject = Instantiate(activeObject);
                    temp.GameObject.transform.position = new Vector2(ray.x, ray.y);
                    levelTiles.Add(temp);

                    Debug.Log("Tile placed at " + ray.x + " " + ray.y + " layer: " + tileLayer);
                    Debug.Log("Number of tiles saved: " + levelTiles.Count);
                }
                else
                {
                    Debug.Log("Tile already exists. Overwriting...");

                    Destroy(temp.GameObject);
                    temp.Tile = new Tile(activeObjectIndex, activeObject, activeObject.name, tileRotation, tileLayer, (int)ray.x, (int)ray.y, activeObjectProperties.Dimensions, activeObjectProperties.Category);
                    temp.GameObject = Instantiate(activeObject);
                    temp.GameObject.transform.position = new Vector2(ray.x, ray.y);

                    Debug.Log("Tile replaced at " + ray.x + " " + ray.y + " layer: " + tileLayer);
                    Debug.Log("Number of tiles saved: " + levelTiles.Count);
                }
            }
            else
            {
                Debug.Log("Editor texture is out of level bounds.");
            }
        }
    }

    public void ClearLevelTiles()
    {
        foreach(CombinedTile item in levelTiles)
        {
            Destroy(item.GameObject);
        }
        levelTiles.Clear();
    }

    //Saving

    public void SaveMapClick()
    {
        if (!string.IsNullOrEmpty(mapname))
        {
            string savefilepath = Application.persistentDataPath + "/" + mapname + ".xml";
            if (File.Exists(savefilepath))
            {
                if (!GameController.GameControllerInstance.AlertPanel.activeSelf)
                {
                    GameController.GameControllerInstance.displayAlert("File exists", "Do you want to overwrite the file?", true);
                    StopCoroutine(AwaitAlertResult());
                    StartCoroutine(AwaitAlertResult());
                }
            }
            else
                SaveMap(savefilepath);
        }
        else
        {
            Debug.Log("Map name can't be empty in order to save it.");
            GameController.GameControllerInstance.displayAlert("Error", "Map name can't be empty in order to be saved.", false);
        }   
    }

    IEnumerator AwaitAlertResult()
    {
        yield return StartCoroutine(GameController.GameControllerInstance.AlertResult());

        if (GameController.GameControllerInstance.Result != null)
        {
            Debug.Log(GameController.GameControllerInstance.Result);
            if (GameController.GameControllerInstance.Result == "OK")
                SaveMap(Application.persistentDataPath + "/" + mapname + ".xml");
            GameController.GameControllerInstance.resetAlertResult();
        }
        yield break;
    }

    public void SaveMap(string savefilepath)
    {
        //string savefilepath = StandaloneFileBrowser.SaveFilePanel("Save the map", "", "New Map", extenstionSave);
        //string savefilepath = EditorUtility.SaveFilePanel("Save the map", Application.dataPath + "/Levels", "New Map", "xml");
        List<SerializableTile> temp = new List<SerializableTile>();
        foreach (CombinedTile item in levelTiles)
        {
            SerializableTile tempTile = new SerializableTile(item.Tile.Index, item.Tile.Name, item.Tile.Rotation, item.Tile.Layer, item.Tile.X, item.Tile.Y, item.Tile.Dimensions, item.Tile.Category);
            temp.Add(tempTile);
        }
        CompleteLevel cl = new CompleteLevel(new MapInfo(mapname, levelWidth, levelHeight), temp);

        FileStream fStream = File.Create(savefilepath);

        //Serialize to xml
        DataContractSerializer bf = new DataContractSerializer(cl.GetType());
        MemoryStream streamer = new MemoryStream();

        //Serialize the file
        bf.WriteObject(streamer, cl);
        streamer.Seek(0, SeekOrigin.Begin);

        //Save to disk
        fStream.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);

        // Close the file to prevent any corruptions
        fStream.Close();

        string result = XElement.Parse(Encoding.ASCII.GetString(streamer.GetBuffer()).Replace("\0", "")).ToString();
        Debug.Log("Saved level. Saved tiles: " + temp.Count + " Serialized Result: " + result);
    }

    //Loading

    public void LoadMapClick()
    {
        if (!GameController.GameControllerInstance.FileDialogPanel.activeSelf)
        {
            GameController.GameControllerInstance.displayFileDialog("Choose a map to load");
            StartCoroutine(AwatFileDialogResult());
            StopCoroutine(AwatFileDialogResult());
        }
    }

    IEnumerator AwatFileDialogResult()
    {
        yield return StartCoroutine(GameController.GameControllerInstance.DialogResult());

        if (GameController.GameControllerInstance.SelectedFile != null)
        {
            if (GameController.GameControllerInstance.SelectedFile != "CANCEL")
            {
                Debug.Log(GameController.GameControllerInstance.SelectedFile);
                LoadMap(Application.persistentDataPath + "/" + GameController.GameControllerInstance.SelectedFile);
            }
            GameController.GameControllerInstance.resetDialogResult();
        }
        yield break;
    }

    public void LoadMap(string loadfilepath)
    {
        try
        {
            //string loadfilepath = StandaloneFileBrowser.OpenFilePanel("Load a map", "", extensionOpen, false)[0];
            //string loadfilepath = EditorUtility.OpenFilePanel("Load a map", Application.dataPath + "/Levels", "xml");
            Debug.Log("Loading level: " + loadfilepath);

            if (File.Exists(loadfilepath))
            {
                FileStream fStream = File.Open(loadfilepath, FileMode.Open, System.IO.FileAccess.Read);

                //Deserialize from xml
                DataContractSerializer serializer = new DataContractSerializer(typeof(CompleteLevel), null, 1000, false, true, null);
                CompleteLevel cl = serializer.ReadObject(fStream) as CompleteLevel;
                fStream.Close();

                SetMapName(cl.MapInfo.MapName);
                ChangeLevelWidth(cl.MapInfo.MapWidth.ToString());
                ChangeLevelHeight(cl.MapInfo.MapHeight.ToString());
                ApplySettings();
                ClearLevelTiles();
                List<SerializableTile> temp = cl.MapTiles;
                foreach(SerializableTile item in temp)
                {
                    Tile tempTile = new Tile(item.Index, levelLoader.tileIndexer[item.Index].Obj, item.Name, item.Rotation, item.Layer, item.X, item.Y, item.Dimensions, item.Category);
                    levelTiles.Add(new CombinedTile(tempTile, null));
                }

                levelLoader.LoadLevel(levelTiles);
                ChangeLayer("0");

                Debug.Log("Loaded " + loadfilepath);
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("Failure during loading the level. Error: " + ex);
        }
    }
}

[DataContract]
public class MapInfo
{
    [DataMember]
    private string mapName;
    [DataMember]
    private int mapWidth;
    [DataMember]
    private int mapHeight;

    public MapInfo(string mapName, int mapWidth, int mapHeight)
    {
        this.mapName = mapName;
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
    }
    public MapInfo()
    {
    }

    public string MapName
    {
        get
        {
            return mapName;
        }

        set
        {
            mapName = value;
        }
    }

    public int MapWidth
    {
        get
        {
            return mapWidth;
        }

        set
        {
            mapWidth = value;
        }
    }

    public int MapHeight
    {
        get
        {
            return mapHeight;
        }

        set
        {
            mapHeight = value;
        }
    }
}
[DataContract]
public class CombinedTile
{
    [DataMember]
    private Tile tile;
    [DataMember]
    private GameObject gameObject;

    public CombinedTile(Tile tile, GameObject gameObject)
    {
        this.tile = tile;
        this.gameObject = gameObject;
    }
    public CombinedTile()
    {
        this.tile = null;
        this.gameObject = null;
    }

    public Tile Tile
    {
        get
        {
            return tile;
        }

        set
        {
            tile = value;
        }
    }

    public GameObject GameObject
    {
        get
        {
            return gameObject;
        }

        set
        {
            gameObject = value;
        }
    }
}
[DataContract]
public class SerializableTile
{
    //Tile index (for saving/loading purposes)
    [DataMember]
    private int index;
    //Tile name; Default none
    [DataMember]
    private string name;
    //The way the object is rotated; Default 0, right =>
    [DataMember]
    private short rotation = 0;
    //Layer the tile is in; Default: 0 =>
    [DataMember]
    private int layer = 0;
    //Coordinates
    [DataMember]
    private int x;
    [DataMember]
    private int y;
    //Boundaries of tile, for example 1x1, 4x2; Default 1x1
    [DataMember]
    private short[] dimensions = new short[2] { 1, 1 };
    //Category; Default none
    [DataMember]
    private string category;

    public SerializableTile(int index, string name, short rotation, int layer, int x, int y, short[] dimensions, string category)
    {
        this.index = index;
        this.name = name;
        this.rotation = rotation;
        this.layer = layer;
        this.x = x;
        this.y = y;
        this.dimensions = dimensions;
        this.category = category;
    }
    public SerializableTile()
    {

    }

    public int Index
    {
        get
        {
            return index;
        }

        set
        {
            index = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public short Rotation
    {
        get
        {
            return rotation;
        }

        set
        {
            rotation = value;
        }
    }

    public int Layer
    {
        get
        {
            return layer;
        }

        set
        {
            layer = value;
        }
    }

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
        }
    }

    public short[] Dimensions
    {
        get
        {
            return dimensions;
        }

        set
        {
            dimensions = value;
        }
    }

    public string Category
    {
        get
        {
            return category;
        }

        set
        {
            category = value;
        }
    }
}
[DataContract]
public class CompleteLevel
{
    [DataMember]
    private MapInfo mapInfo;
    [DataMember]
    private List<SerializableTile> mapTiles;

    public CompleteLevel(MapInfo mapInfo, List<SerializableTile> mapTiles)
    {
        this.MapInfo = mapInfo;
        this.MapTiles = mapTiles;
    }
    public CompleteLevel()
    {
        this.MapInfo = null;
        this.MapTiles = null;
    }

    public MapInfo MapInfo
    {
        get
        {
            return mapInfo;
        }

        set
        {
            mapInfo = value;
        }
    }

    public List<SerializableTile> MapTiles
    {
        get
        {
            return mapTiles;
        }

        set
        {
            mapTiles = value;
        }
    }
}
