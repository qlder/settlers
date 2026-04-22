using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Newtonsoft.Json;


[System.Serializable]
public class MapData {

    public Dictionary<string, Tile> tiles = new();

    public static MapData Inst() {
        return Game.Inst.data.mapData;
    }

}
