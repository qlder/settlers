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


        int mapSize = game.data.gameOptions.mapSize;
        for (int x = 0; x < mapSize; x++) {
            for (int z = 0; z < mapSize; z++) {
                Tile tile = new Tile();
                tile.key = $"{x}_{z}";
                tile.position = new int2(x, z);
                tile.groundType = GroundType.Earth;
                tile.sectorId = "";
                if (x < mapSize / 4 || x > mapSize * 3 / 4 || z < mapSize / 4 || z > mapSize * 3 / 4) {
                    tile.groundType = GroundType.Water;
                }
                if (x > 10 && z == 10 && x % 50 != 0) {
                    tile.groundType = GroundType.Earth;
                }

                mapData.tiles[tile.key] = tile;
            }
        }
    }

}
