using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

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
    private int activeTileIndex;
    private GameObject activeObject; //active tile to place

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
    //The text that shows tile coordinates
    private Text coordinatesText;
    //The text that shows tile layer
    private Text layerText;
    //Regex for only numbers
    Regex numberRGX = new Regex("^[0-9]$");

    //Main
    //Tile object in prefab panel
    public GameObject menutile;
    int editorTileCount;
    //List of tiles in level
    List<Tile> levelTiles = new List<Tile>();
    //the list of available prefabs is contained in LevelLoader


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

        if (Input.GetMouseButtonDown(0)) Debug.Log("Pressed left click.");
        if (Input.GetMouseButtonDown(1))
        {
            ResetActiveTile();
        }
        if (Input.GetMouseButtonDown(2)) Debug.Log("Pressed middle click.");
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
            tileScriptList[i].index = prefTileListIndex + i;

            if (prefTileListIndex + i < levelLoader.prefabTilesList.Count)
                tileScriptList[i].changeSelf(levelLoader.prefabTilesList[prefTileListIndex]);
            else
                tileScriptList[i].resetSelf();
        }
        Debug.Log("index in list:" + prefTileListIndex + " number of prefabs found: " + levelLoader.prefabTilesList.Count);
    }

    public void changeListIndex()
    {

        prefTileListIndex -= (int)(Input.GetAxis("Mouse ScrollWheel") * 10);
        if (prefTileListIndex > levelLoader.prefabTilesList.Count - 1)
            prefTileListIndex = levelLoader.prefabTilesList.Count - 1;
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
        mapname = name;
    }

    public void ChangeLevelWidth(string width)
    {
        if (numberRGX.IsMatch(width))
        {
            int.TryParse(width, out levelWidth);
        }
    }

    public void ChangeLevelHeight(string height)
    {
        if (numberRGX.IsMatch(height))
        {
            int.TryParse(height, out levelHeight);
        }
    }

    public void ChangeLayer(string layer)
    {
        if (numberRGX.IsMatch(layer))
        {
            int.TryParse(layer, out tileLayer);
            layerText.text = "Layer: " + tileLayer;
        }
    }

    public void ApplySettings()
    {
        resizeLevelBoundaries();
    }

    public void ChangeActiveTile(int ind, GameObject obj)
    {
        activeTileIndex = ind;
        activeObject = obj;
        SpriteRenderer newSprite = obj.GetComponent<SpriteRenderer>();
        Debug.Log("selected tile with index: " + ind + " obj: " + obj.name + " sprite renderer: " + newSprite.sprite);
        editorTexture.transform.localScale = new Vector3(1, 1, 1);
        editorTextureRenderer.sprite = newSprite.sprite;
        editorTextureRenderer.color = newSprite.color;

        tileRotation = 0;
    }

    public void ResetActiveTile()
    {
        activeTileIndex = -1;
        activeObject = null;
        editorTexture.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        editorTextureRenderer.sprite = editorDefaultSprite;
        editorTextureRenderer.color = editorDefaultColor;

        Debug.Log("Resetting editor tile to: " + editorDefaultSprite + " " + editorDefaultColor);
    }
}
