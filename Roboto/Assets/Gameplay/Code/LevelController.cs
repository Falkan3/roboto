using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

public class LevelController : MonoBehaviour {
    private LevelLoader levelLoader;
    private string levelToLoadPath;
    //List of tiles in level, it stores the tile class object and corresponding gameobject
    public List<CombinedTile> levelTiles = new List<CombinedTile>();

    void Start ()
    {
        levelLoader = GetComponent<LevelLoader>();
        levelToLoadPath = GameController.GameControllerInstance.SelectedLevelPath;
        if (string.IsNullOrEmpty(levelToLoadPath))
            GameController.GameControllerInstance.GM_LoadScene("1");

        LoadMap(levelToLoadPath);  
	}

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.GameControllerInstance.GM_LoadScene("1");
        }
    }

    public void LoadMap(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                FileStream fStream = File.Open(path, FileMode.Open, System.IO.FileAccess.Read);

                //Deserialize from xml
                DataContractSerializer serializer = new DataContractSerializer(typeof(CompleteLevel), null, 1000, false, true, null);
                CompleteLevel cl = serializer.ReadObject(fStream) as CompleteLevel;
                fStream.Close();

                List<SerializableTile> temp = cl.MapTiles;
                foreach (SerializableTile item in temp)
                {
                    Tile tempTile = new Tile(item.Index, levelLoader.tileIndexer[item.Index].Obj, item.Name, item.Rotation, item.Layer, item.X, item.Y, item.Dimensions, item.Category);
                    levelTiles.Add(new CombinedTile(tempTile, null));
                }

                levelLoader.LoadLevel(levelTiles);

                Debug.Log("Loaded " + path);
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("Failure during loading the level. Error: " + ex);
            GameController.GameControllerInstance.GM_LoadScene("1");
        }
    }


}
