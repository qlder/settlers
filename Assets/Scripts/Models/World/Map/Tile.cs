using Unity.Mathematics;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class Tile {

    public string key;
    public int2 position;
    public GroundType groundType;

    public string sectorId;

    [JsonIgnore]
    public float2 Center => position + new float2(0.5f, 0.5f);

    //Helpers
    public Sector GetSector() {
        return MapData.Inst().sectors[sectorId];
    }

    public List<Tile> GetConnectedTiles() {
        return MapData.Inst().tiles.Values.Where(t => t.sectorId == this.sectorId).ToList();
    }


}
