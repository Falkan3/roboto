using UnityEngine;
using System.Collections;

public class Tile {
    //Tile object
    private GameObject tileObject;
    //Tile name; Default none
    private string name;
    //The way the object is rotated; Default 0, right =>
    private short rotation = 0;
    //Layer the tile is in; Default: 0 =>
    private int layer = 0;
    //Coordinates
    private int x;
    private int y;
    //Boundaries of tile, for example 1x1, 4x2; Default 1x1
    private short[] dimensions = new short[2] { 1, 1 };
    //Category; Default none
    private string category;

    public Tile(GameObject tileobj, string name, short rotation, int layer, int x, int y, short[] dimensions, string category)
    {
        this.tileObject = tileobj; this.name = name; this.rotation = rotation; this.layer = layer; this.x = x; this.y = y; this.dimensions = dimensions; this.category = category;
    }

    public GameObject TileObject
    {
        get
        {
            return tileObject;
        }

        set
        {
            tileObject = value;
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
