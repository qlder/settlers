using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pos
{
    public float X { get; private set; }
    public float Z { get; private set; }

    public Pos(float x, float z)
    {
        X = x;
        Z = z;
    }

    public GetTile()
    {
        return Map.Inst().tiles[X, Z] ?? null;
    }


}
