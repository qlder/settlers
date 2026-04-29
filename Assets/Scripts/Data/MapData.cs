using System.Collections.Generic;

[System.Serializable]
public class MapData {
    public Dictionary<string, Tile> tiles = new();

    public Dictionary<int, Sector> sectors = new();

    // chunkKey -> sector ids inside that 16x16 chunk
    public Dictionary<string, List<int>> chunkSectorIds = new();

    // linkHash -> sector ids touching that link
    public Dictionary<long, HashSet<int>> linkHashToSectorIds = new();

    private int nextSectorId = 1;
    private int nextConnectivityId = 1;

    public static MapData Inst() {
        return Game.Inst.data.mapData;
    }

    public int GetNextSectorId() {
        return nextSectorId++;
    }

    public int GetNextConnectivityId() {
        return nextConnectivityId++;
    }

    public void ResetSectorIds() {
        nextSectorId = 1;
        nextConnectivityId = 1;
    }

    public void ClearSectorData() {
        sectors.Clear();
        chunkSectorIds.Clear();
        linkHashToSectorIds.Clear();
        ResetSectorIds();
    }

    public void ResetConnectivityIds() {
        nextConnectivityId = 1;
    }
}