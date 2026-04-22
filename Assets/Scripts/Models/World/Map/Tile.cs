using Unity.Mathematics;
using UnityEngine;

public struct Tile {

    public string key;
    public int2 position;
    public GroundType groundType;

    public float2 Center => position + new float2(0.5f, 0.5f);



}
