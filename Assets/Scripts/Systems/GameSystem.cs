using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;

public class GameSystem {

    public HumanSystem humanSystem = new();
    public SectorSystem sectorSystem = new();
    public PathSystem pathSystem = new();

    public static GameSystem Inst() {
        return Game.Inst.system;
    }

    public void Tick(int ticks) {
        humanSystem.Tick(ticks);
        if (Rng.Inst().Float(0f, 1f) < 0.01f) {
            sectorSystem.RebuildAll();
            Sector sector = MapData.Inst().sectors.GetRandom().Value;
            Debug.Log($"Random sector: {sector.id} with connectivity {sector.connectivityId}");
            Tile tile1 = MapData.Inst().tiles[sector.tileKeys.GetRandom()];
            Tile tile2 = MapData.Inst().tiles[sector.tileKeys.GetRandom()];
            Debug.Log($"Random tiles: {tile1.key} and {tile2.key} in sector {sector.id} with connectivity {sector.connectivityId}");

            Queue<Tile> path = pathSystem.FindPath(tile1, tile2);
            if (path != null) {
                Debug.Log($"Found path between {tile1.key} and {tile2.key} with length {path.Count}");
            } else {
                Debug.Log($"No path found between {tile1.key} and {tile2.key}");
            }

        }
    }

}
