using UnityEngine;

[System.Serializable]
public class Map {

    public const int TERRAIN_SIZE = 128;
    public Tile[,] tiles;
    public TileCorner[,] tileCorners;

    public int terrainsLength;
    public int tileLength;

    public static Map Inst() {
        return Game.Inst.data.map;
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
