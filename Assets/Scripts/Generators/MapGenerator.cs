using System;
using UnityEngine;
using Unity.Mathematics;

public class MapGenerator {

    public void Generate(Game game) {
        GenerateTerrain(game);
    }

    private void GenerateTerrain(Game game) {
        MapData mapData = new MapData();
        game.data.mapData = mapData;


        int mapSize = game.data.gameOptions.MapSize;
        for (int x = 0; x < mapSize; x++) {
            for (int z = 0; z < mapSize; z++) {
                Tile tile = new Tile();
                tile.Key = $"{x}_{z}";
                tile.X = x;
                tile.Y = z;
                tile.GroundType = GroundType.Earth;
                tile.SectorKey = "";
                if (x < mapSize / 4 || x > mapSize * 3 / 4 || z < mapSize / 4 || z > mapSize * 3 / 4) {
                    tile.GroundType = GroundType.Water;
                }
                if (x > 10 && z == 10 && x % 50 != 0) {
                    tile.GroundType = GroundType.Earth;
                }

                mapData.tiles[tile.Key] = tile;
            }
        }
    }

}
