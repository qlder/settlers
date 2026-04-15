using UnityEngine;

[System.Serializable]
public class TileCorner {

    public int X;
    public int Z;
    public int H;

    public static TileCorner Get(int x, int z) {
        return Map.Inst().tileCorners[x, z] ?? null;
    }

    public void SetHeight(int height) {
        H = height;
    }


}