using System.Collections.Generic;
using UnityEngine;

public class MapFactory {


    public static Map CreateMap(GameOptions options) {
        Map map = new Map();
        map.tileLength = Map.TERRAIN_SIZE * options.mapSize;
        map.tiles = new Tile[map.tileLength, map.tileLength];
        map.tileCorners = new TileCorner[map.tileLength + 1, map.tileLength + 1];
        map.terrainsLength = options.mapSize;

        for (int x = 0; x < map.tileLength; x++) {
            for (int z = 0; z < map.tileLength; z++) {
                map.tiles[x, z] = CreateTile(x, z);
            }
        }

        for (int x = 0; x < map.tileLength + 1; x++) {
            for (int z = 0; z < map.tileLength + 1; z++) {
                map.tileCorners[x, z] = CreateTileCorner(x, z, 0);
            }
        }
        return map;
    }

    private static Tile CreateTile(int x, int z) {
        Tile tile = new Tile();
        tile.X = x;
        tile.Z = z;
        tile.isWalkable = true;
        return tile;
    }

    private static TileCorner CreateTileCorner(int x, int z, int h) {
        TileCorner tileCorner = new TileCorner();
        tileCorner.X = x;
        tileCorner.Z = z;
        tileCorner.H = h;
        return tileCorner;
    }


}
