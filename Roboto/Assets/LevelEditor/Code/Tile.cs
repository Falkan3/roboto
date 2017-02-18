using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

[DataContract]
public class Tile {
    //Tile object
    private GameObject tileObject;
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

    public Tile(int index, GameObject tileobj, string name, short rotation, int layer, int x, int y, short[] dimensions, string category)
    {
        this.index = index;  this.tileObject = tileobj; this.name = name; this.rotation = rotation; this.layer = layer; this.x = x; this.y = y; this.dimensions = dimensions; this.category = category;
    }
    public Tile()
    {

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
