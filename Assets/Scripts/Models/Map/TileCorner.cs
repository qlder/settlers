using UnityEngine;

public class TileCorner {

    public int X { get; private set; }
    public int Z { get; private set; }
    public int H { get; private set; }

    public TileCorner(int x, int z, int h) {
        X = x;
        Z = z;
        H = h;
    }

    public static TileCorner Get(int x, int z) {
        return Map.Inst().tileCorners[x, z] ?? null;
    }

    public void SetHeight(int height) {
        H = height;
    }


}