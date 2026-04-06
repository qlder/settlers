using UnityEngine;

public class Map {

    public const int TERRAIN_SIZE = 128;
    public Tile[,] tiles;
    public TileCorner[,] tileCorners;

    public int terrainsLength { get; private set; }
    public int tileLength { get; private set; }

    public static Map Inst() {
        return Game.Inst.map;
    }

    public Map(GameOptions options) {
        this.tileLength = TERRAIN_SIZE * options.mapSize;
        tiles = new Tile[tileLength, tileLength];
        tileCorners = new TileCorner[tileLength + 1, tileLength + 1];
        terrainsLength = options.mapSize;

        for (int x = 0; x < tileLength; x++) {
            for (int z = 0; z < tileLength; z++) {
                tiles[x, z] = new Tile(x, z);
            }
        }

        for (int x = 0; x < tileLength + 1; x++) {
            for (int z = 0; z < tileLength + 1; z++) {
                tileCorners[x, z] = new TileCorner(x, z, 0);
            }
        }
    }

    public Tile GetRandomNearbyTile(Tile currentTile, int minDistance, int maxDistance) {
        for (int i = 0; i < 10; i++) {
            int xOffset = UnityEngine.Random.Range(-maxDistance, maxDistance + 1);
            int zOffset = UnityEngine.Random.Range(-maxDistance, maxDistance + 1);

            // Enforce minimum distance (square ring, not full square)
            if (Mathf.Abs(xOffset) < minDistance && Mathf.Abs(zOffset) < minDistance)
                continue;

            int newX = currentTile.X + xOffset;
            int newZ = currentTile.Z + zOffset;

            if (newX >= 0 && newX < tileLength && newZ >= 0 && newZ < tileLength) {
                if (!tiles[newX, newZ].isWalkable) {
                    continue;
                }
                return tiles[newX, newZ];
            }
        }

        return null;
    }



}
