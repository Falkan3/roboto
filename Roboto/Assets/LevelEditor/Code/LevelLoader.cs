using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {
    public List<Tile> tileList = new List<Tile>();
    public List<GameObject> prefabTilesList = new List<GameObject>();
    public List<TileIndexer> tileIndexer = new List<TileIndexer>();

    [System.Serializable]
    public class TileIndexer
    {
        [SerializeField]
        private int index;
        [SerializeField]
        private GameObject obj;

        public TileIndexer(int index, GameObject obj)
        {
            this.index = index;
            this.obj = obj;
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

        public GameObject Obj
        {
            get
            {
                return obj;
            }

            set
            {
                obj = value;
            }
        }
    }

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
            Debug.Log("Found " + prefabTiles[i].name);
        }

        Debug.Log("Loaded " + prefabTiles.Length + " Tiles");
    }

    public void LoadLevel(List<CombinedTile> levelTiles)
    {
        for(int i=0;i<levelTiles.Count;i++)
        {
            PlaceTile(levelTiles[i]);
        }
    }

    // Level loader

    public void PlaceTile(CombinedTile tile)
    {
        tile.GameObject = Instantiate(tile.Tile.TileObject);
        tile.GameObject.transform.position = new Vector2(tile.Tile.X, tile.Tile.Y);
        tile.GameObject.transform.rotation = new Quaternion(0, 0, 0, 1);
        tile.GameObject.transform.Rotate(0, 0, 90 * tile.Tile.Rotation);
        if (tile.Tile.Category == "ConveyorBelt")
            tile.GameObject.GetComponent<ConveyorBelt>().TileRotation = tile.Tile.Rotation;

        Debug.Log("Tile " + tile.Tile.TileObject + " placed at " + tile.Tile.X + " " + tile.Tile.Y + " layer: " + tile.Tile.Layer);
     }
}
