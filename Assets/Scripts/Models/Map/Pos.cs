using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public class Pos
{

    public float X { get; private set; }
    public float Z { get; private set; }

    [JsonConstructor]
    public Pos(float x, float z)
    {
        X = x;
        Z = z;
    }

    public Pos(Vector2 vector)
    {
        X = vector.x;
        Z = vector.y;
    }

    public Tile GetTile()
    {
        return Map.Inst().tiles[(int)X, (int)Z] ?? null;
    }

    public Vector2 GetVector2()
    {
        return new Vector2(X, Z);
    }



    // public Vector3 GetVector3() {
    //     int X = (int)this.X;
    //     int Z = (int)this.Z;
    //     float height = 0;
    //     height += TileCorner.Get(X + 0, Z + 0).H;
    //     height += TileCorner.Get(X + 1, Z + 0).H;
    //     height += TileCorner.Get(X + 0, Z + 1).H;
    //     height += TileCorner.Get(X + 1, Z + 1).H;
    //     height *= 0.25f;
    //     return new Vector3(X + 0.5f, height, Z + 0.5f);
    // }

    public Vector3 GetVector3()
    {
        int X = (int)this.X;
        int Z = (int)this.Z;
        float tx = this.X - X;
        float tz = this.Z - Z;

        float h00 = TileCorner.Get(X + 0, Z + 0).H; // bottom-left
        float h10 = TileCorner.Get(X + 1, Z + 0).H; // bottom-right
        float h01 = TileCorner.Get(X + 0, Z + 1).H; // top-left
        float h11 = TileCorner.Get(X + 1, Z + 1).H; // top-right

        if (h00 == h10 && h00 == h01 && h00 == h11)
        {
            return new Vector3(X + tx, h00, Z + tz);
        }

        float height =
            h00 * (1 - tx) * (1 - tz) +
            h10 * tx * (1 - tz) +
            h01 * (1 - tx) * tz +
            h11 * tx * tz;

        return new Vector3(X + tx, height, Z + tz);
    }


}
