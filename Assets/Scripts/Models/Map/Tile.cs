using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile {
    public int X { get; private set; }
    public int Z { get; private set; }

    bool isWalkable = true; //Change later...

    public Tile(int x, int z) {
        X = x;
        Z = z;
    }

    public enum Flatness {
        Flat,
        Gentle,
        Sloped,
    }

    public static Tile Get(int x, int z) {
        return Map.Inst().tiles[x, z] ?? null;
    }

    public Pos GetPosition() {
        return new Pos(X + 0.5f, Z + 0.5f);
    }



    // public List<TileCorner> GetCorners() {
    //     List<TileCorner> corners = new List<TileCorner>();
    //     corners.Add(TileCorner.Get(X, Z));
    //     corners.Add(TileCorner.Get(X + 1, Z));
    //     corners.Add(TileCorner.Get(X, Z + 1));
    //     corners.Add(TileCorner.Get(X + 1, Z + 1));
    //     return corners;
    // }



}
