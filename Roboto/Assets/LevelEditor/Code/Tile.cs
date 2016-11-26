using UnityEngine;
using System.Collections;

public class Tile {
    //Tile object
    public GameObject tileObject;
    //Tile name; Default none
    public string name;
    //The way the object is rotated; Default 0, right =>
    public short rotation = 0;
    //Layer the tile is in; Default: 0 =>
    public short layer = 0;
    //Coordinates
    public short x;
    public short y;
    //Boundaries of tile, for example 1x1, 4x2; Default 1x1
    public short[] dimensions = new short[2]{ 1, 1 };
    //Category; Default none
    public string category;
}
