using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetActiveTile : MonoBehaviour {
    public int index;
    public GameObject tile;
    private Button button;
    private LevelEditorController editor;

    void Start () {
        editor = GameObject.Find("SceneManager").GetComponent<LevelEditorController>();
    }

    public void setTile()
    {
        editor.ChangeActiveTile(index, tile);
    }

    public void changeSelf(GameObject obj)
    {
        tile = obj;
        Debug.Log(obj.name);
        GetComponent<Button>().image.sprite = obj.GetComponent<SpriteRenderer>().sprite;
    }
    public void resetSelf()
    {
        tile = null;
        GetComponent<Button>().image.sprite = null;
    }
}
