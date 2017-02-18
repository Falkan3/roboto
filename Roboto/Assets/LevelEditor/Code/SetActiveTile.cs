using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetActiveTile : MonoBehaviour {
    private int index;
    private int tileIndex;
    private GameObject tile;
    private LevelEditorController editor;

    void Start () {
        editor = GameObject.Find("SceneManager").GetComponent<LevelEditorController>();
    }

    public void setTile()
    {
        editor.ChangeActiveTile(index, tile, tileIndex);
    }

    public void changeSelf(GameObject obj, int tileIndex)
    {
        tile = obj;
        Debug.Log(obj.name);
        GetComponent<Button>().image.sprite = obj.GetComponent<SpriteRenderer>().sprite;
        this.tileIndex = tileIndex;
    }
    public void resetSelf()
    {
        tile = null;
        tileIndex = -1;
        GetComponent<Button>().image.sprite = null;
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

    public GameObject Tile
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

    public int TileIndex
    {
        get
        {
            return tileIndex;
        }

        set
        {
            tileIndex = value;
        }
    }
}
