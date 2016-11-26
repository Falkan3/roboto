using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {
    public List<Tile> tileList = new List<Tile>();
    public List<GameObject> prefabTilesList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        LoadPrefabs();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadPrefabs(string mainfolder = "Prefabs/Tiles")
    {
        GameObject[] prefabTiles;
        prefabTiles = Resources.LoadAll<GameObject>(mainfolder + "");

        for (int i = 0; i < prefabTiles.Length; i++)
        {
            prefabTilesList.Add(prefabTiles[i]);
        }

        Debug.Log("Loaded " + prefabTiles.Length + " Tiles");
    }
}
