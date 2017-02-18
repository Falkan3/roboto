using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EditorTile {
    //Tile index (for saving/loading)
    [SerializeField]
    private int index = -1;
    //Tile name; Default none
    [SerializeField]
    private string name;
    //Boundaries of tile, for example 1x1, 4x2; Default 1x1
    [SerializeField]
    private short[] dimensions = new short[2] { 1, 1 };
    //Category; Default none
    [SerializeField]
    private string category;

    public EditorTile(string name, short[] dimensions, string category)
    {
        this.name = name; this.dimensions = dimensions; this.category = category;
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
}
