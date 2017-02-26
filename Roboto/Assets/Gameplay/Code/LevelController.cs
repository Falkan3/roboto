﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class LevelController : MonoBehaviour {
    private LevelLoader levelLoader;
    private string levelToLoadPath;
    //List of tiles in level, it stores the tile class object and corresponding gameobject
    public List<CombinedTile> levelTiles = new List<CombinedTile>();

    //Prefabs in the scene
    public GameObject Player;
    public CombinedTile[] Crates;
    public CombinedTile[] FinishTiles;
    private bool[] WinArray;

    void Start()
    {
        levelLoader = GetComponent<LevelLoader>();
        levelToLoadPath = GameController.GameControllerInstance.SelectedLevelPath;
        if (string.IsNullOrEmpty(levelToLoadPath))
            GameController.GameControllerInstance.GM_LoadScene("1");

        LoadMap(levelToLoadPath);
        MapInit(); 
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(UnityEditor.EditorUtility.DisplayDialog("Return to menu", "Do you want to return to the main menu?", "Yes", "No"))
                ReturnToMenu();
        }
            
    }

    void ReturnToMenu(string error = null)
    {
        if(error != null)
        {
            UnityEditor.EditorUtility.DisplayDialog("Error!", error, "Return to menu");
        }
        GameController.GameControllerInstance.GM_LoadScene("1");
    }

    void WinGame()
    {
        UnityEditor.EditorUtility.DisplayDialog("Error!", "Congratulations! You have completed this level successfully.", "Return to menu");
        GameController.GameControllerInstance.GM_LoadScene("1");
    }

    void LoadMap(string path)
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

    void MapInit()
    {
        if (Player != null)
        {
            IEnumerable<CombinedTile> SpawnTiles = levelTiles.Where(obj => obj.Tile.Category == "Respawn");
            if (SpawnTiles.Count() > 0)
            {
                int rand_index = new System.Random().Next(0, SpawnTiles.Count());
                Player.transform.position = SpawnTiles.ElementAt(rand_index).GameObject.transform.position;
                Player.SetActive(true);
            }
            else
            {
                ReturnToMenu("No Respawn tiles found.");
            }      
        }
        else
        {
            ReturnToMenu("Player is null.");
        }

        IEnumerable<CombinedTile> cratesEnumQuery = levelTiles.Where(obj => obj.Tile.Category == "Crate");
        if (cratesEnumQuery.Count() > 0)
        {
            Crates = cratesEnumQuery.ToArray();
        }
        else
        {
            ReturnToMenu("No Crates found.");
        }

        IEnumerable<CombinedTile> finishTilesEnumQuery = levelTiles.Where(obj => obj.Tile.Category == "Finish");
        if (finishTilesEnumQuery.Count() > 0)
        {
            FinishTiles = finishTilesEnumQuery.ToArray();
            WinArray = new bool[FinishTiles.Length];
            for (int i = 0; i < WinArray.Length; i++)
                WinArray[i] = false;
        }
        else
        {
            ReturnToMenu("No Finish tiles found.");
        }

        //Invoke function checking for victory
        InvokeRepeating("MapCheckWin", 0f, 1.0f);
    }

    void MapCheckWin()
    {
        Debug.Log("Checking for win...");
        Debug.Log(FinishTiles[1].GameObject.name);
        string temp = "";
        bool win = true;

        for(int i=0; i<FinishTiles.Length; i++)
        {
            for(int j=0; j<Crates.Length; j++)
            {
                float distance = Vector2.Distance(Crates[j].GameObject.transform.position, FinishTiles[i].GameObject.transform.position);
                Debug.Log("Tile " + FinishTiles[i].GameObject.name + " distance to " + Crates[j].GameObject.name + " is: " + distance);
                if (distance <= 0.1f)
                {
                    WinArray[i] = true;
                    break;
                }
                else
                    WinArray[i] = false;
            }
        }
        for (int i = 0; i < WinArray.Length; i++)
        {
            temp += WinArray[i].ToString() + " ";
        }
        for (int i = 0; i<WinArray.Length; i++)
        {
            if (WinArray[i] == false)
            {
                win = false;
                break;
            }
                
        }
        temp += win.ToString();
        Debug.Log(temp);
        if (win)
            WinGame();
    }
}
