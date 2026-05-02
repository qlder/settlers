using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;

public class GameSystem {

    public HumanSystem humanSystem = new();
    public JobSystem jobSystem = new();
    public SectorSystem sectorSystem = new();
    public PathSystem pathSystem = new();

    public static GameSystem Inst() {
        return Game.Inst.system;
    }

    public void Setup() {
        sectorSystem.RebuildAll();
    }

    public void Tick(int ticks) {
        humanSystem.Tick(ticks);
        jobSystem.Tick(ticks);
        if (Rng.Inst().Float(0f, 1f) < 0.01f) {

            // // Sector sector = MapData.Inst().sectors.GetRandom().Value;
            // // Debug.Log($"Random sector: {sector.id} with connectivity {sector.connectivityId}");
            // Tile tile1 = MapData.Inst().tiles.Values.GetRandom();
            // Tile tile2 = MapData.Inst().tiles.Values.GetRandom();

            // Sector sector1 = MapData.Inst().sectors[tile1.sectorId];
            // Sector sector2 = MapData.Inst().sectors[tile2.sectorId];

            // Debug.Log($"Random tiles: {tile1.key}={tile1.sectorId}={sector1.connectivityId} and {tile2.key}={tile2.sectorId}={sector2.connectivityId}");

            // Path path = pathSystem.FindPath(tile1, tile2);
            // if (path.status == Path.PathStatus.Complete) {
            //     Debug.Log($"Found path between {tile1.key} and {tile2.key} with length {path.tile_keys.Count}");
            // } else {
            //     Debug.Log($"No path found between {tile1.key} and {tile2.key}");
            // }

        }
    }

}
