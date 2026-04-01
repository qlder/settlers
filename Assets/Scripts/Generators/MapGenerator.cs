using System;
using UnityEngine;

public class MapGenerator {

    public static void Generate(Game game) {
        GenerateTerrain(game);
    }

    private static void GenerateTerrain(Game game) {
        Map map = game.map;
        int cornerLength = map.tileLength + 1;

        for (int x = 0; x < cornerLength; x++) {
            for (int z = 0; z < cornerLength; z++) {
                TileCorner corner = TileCorner.Get(x, z);
                corner.SetHeight(Mathf.RoundToInt(Mathf.PerlinNoise(x * 0.01f, z * 0.01f) * 10f));
            }
        }
    }

}
