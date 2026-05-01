using System.Collections.Generic;
using Unity.Mathematics;

public class SectorSystem {
    public const int ChunkSize = 16;

    private readonly Dictionary<string, List<string>> chunkSectorIds = new();
    private readonly Dictionary<long, HashSet<string>> linkHashToSectorIds = new();

    private int nextConnectivityId = 0;

    private static readonly int2[] CardinalDirections =
    {
        new int2(1, 0),
        new int2(-1, 0),
        new int2(0, 1),
        new int2(0, -1),
    };

    private static readonly int2[] LinkDirections =
    {
        new int2(1, 0),
        new int2(0, 1),
    };

    public void RebuildAll() {
        ClearSectorData();
        ClearAllTileSectorIds();

        Dictionary<string, int2> chunks = GetAllChunkCoords();

        foreach (int2 chunkCoord in chunks.Values) {
            BuildChunkSectors(chunkCoord);
        }

        RebuildAllSectorLinks();
        RebuildConnectivityIds();
    }

    private void ClearSectorData() {
        MapData.Inst().sectors.Clear();

        chunkSectorIds.Clear();
        linkHashToSectorIds.Clear();

        ResetConnectivityIds();
    }

    private int GetNextConnectivityId() {
        return nextConnectivityId++;
    }

    private void ResetConnectivityIds() {
        nextConnectivityId = 0;
    }

    public void RebuildChunkContainingTile(string tileKey) {
        if (!MapData.Inst().tiles.TryGetValue(tileKey, out Tile tile))
            return;

        int2 chunkCoord = GetChunkCoord(tile.position);

        RemoveChunkSectors(chunkCoord);
        ClearChunkTileSectorIds(chunkCoord);

        BuildChunkSectors(chunkCoord);

        RebuildAllSectorLinks();
        RebuildConnectivityIds();
    }

    public void RebuildChunk(int2 chunkCoord) {
        RemoveChunkSectors(chunkCoord);
        ClearChunkTileSectorIds(chunkCoord);

        BuildChunkSectors(chunkCoord);

        RebuildAllSectorLinks();
        RebuildConnectivityIds();
    }

    private void BuildChunkSectors(int2 chunkCoord) {
        string chunkKey = ChunkKey(chunkCoord);

        if (!chunkSectorIds.ContainsKey(chunkKey))
            chunkSectorIds[chunkKey] = new List<string>();

        HashSet<string> visited = new();

        int localSectorIndex = 0;

        foreach (string tileKey in GetTileKeysInChunk(chunkCoord)) {
            if (visited.Contains(tileKey))
                continue;

            Tile tile = MapData.Inst().tiles[tileKey];

            visited.Add(tileKey);

            Sector sector = FloodFillSectorInsideChunk(
                tileKey,
                chunkCoord,
                visited,
                localSectorIndex
            );

            MapData.Inst().sectors[sector.id] = sector;
            chunkSectorIds[chunkKey].Add(sector.id);

            localSectorIndex++;
        }
    }

    private Sector FloodFillSectorInsideChunk(
        string startTileKey,
        int2 chunkCoord,
        HashSet<string> visited,
        int localSectorIndex
    ) {
        Tile startTile = MapData.Inst().tiles[startTileKey];

        Sector sector = new() {
            id = SectorId(chunkCoord, localSectorIndex),
            connectivityId = "",
            chunkCoord = chunkCoord,
            passability = GetPassability(startTile)
        };

        Queue<string> queue = new();
        queue.Enqueue(startTileKey);

        while (queue.Count > 0) {
            string tileKey = queue.Dequeue();

            Tile tile = MapData.Inst().tiles[tileKey];

            tile.sectorId = sector.id;
            MapData.Inst().tiles[tileKey] = tile;

            sector.tileKeys.Add(tileKey);

            foreach (int2 direction in CardinalDirections) {
                int2 neighborPosition = tile.position + direction;

                if (!IsInsideChunk(neighborPosition, chunkCoord))
                    continue;

                string neighborKey = TileKey(neighborPosition);

                if (!MapData.Inst().tiles.TryGetValue(neighborKey, out Tile neighbor))
                    continue;

                if (visited.Contains(neighborKey))
                    continue;

                if (GetPassability(neighbor) != sector.passability)
                    continue;

                visited.Add(neighborKey);
                queue.Enqueue(neighborKey);
            }
        }

        return sector;
    }

    private void RebuildAllSectorLinks() {
        linkHashToSectorIds.Clear();

        foreach (Sector sector in MapData.Inst().sectors.Values) {
            sector.neighborSectorIds.Clear();
            sector.linkHashes.Clear();
        }

        List<string> tileKeys = new(MapData.Inst().tiles.Keys);

        foreach (string tileKey in tileKeys) {
            Tile tile = MapData.Inst().tiles[tileKey];

            if (string.IsNullOrEmpty(tile.sectorId))
                continue;

            foreach (int2 direction in LinkDirections) {
                int2 neighborPosition = tile.position + direction;
                string neighborKey = TileKey(neighborPosition);

                if (!MapData.Inst().tiles.TryGetValue(neighborKey, out Tile neighbor))
                    continue;

                if (string.IsNullOrEmpty(neighbor.sectorId))
                    continue;

                if (neighbor.sectorId == tile.sectorId)
                    continue;

                Sector a = MapData.Inst().sectors[tile.sectorId];
                Sector b = MapData.Inst().sectors[neighbor.sectorId];

                if (a.passability != b.passability)
                    continue;

                long linkHash = LinkHash(tile.position, neighbor.position);

                RegisterSectorLink(a.id, linkHash);
                RegisterSectorLink(b.id, linkHash);
            }
        }
    }

    private void RegisterSectorLink(string sectorId, long linkHash) {
        if (!MapData.Inst().sectors.TryGetValue(sectorId, out Sector sector))
            return;

        sector.linkHashes.Add(linkHash);

        if (!linkHashToSectorIds.TryGetValue(linkHash, out HashSet<string> linkedSectorIds)) {
            linkedSectorIds = new HashSet<string>();
            linkHashToSectorIds[linkHash] = linkedSectorIds;
        }

        foreach (string otherSectorId in linkedSectorIds) {
            if (otherSectorId == sectorId)
                continue;

            sector.neighborSectorIds.Add(otherSectorId);

            if (MapData.Inst().sectors.TryGetValue(otherSectorId, out Sector otherSector))
                otherSector.neighborSectorIds.Add(sectorId);
        }

        linkedSectorIds.Add(sectorId);
    }

    private void RebuildConnectivityIds() {
        ResetConnectivityIds();

        HashSet<string> visited = new();
        List<string> sectorIds = new(MapData.Inst().sectors.Keys);

        foreach (string sectorId in sectorIds) {
            if (visited.Contains(sectorId))
                continue;

            string connectivityId = $"c_{GetNextConnectivityId()}";

            Queue<string> queue = new();
            queue.Enqueue(sectorId);
            visited.Add(sectorId);

            while (queue.Count > 0) {
                string currentId = queue.Dequeue();

                Sector current = MapData.Inst().sectors[currentId];
                current.connectivityId = connectivityId;

                foreach (string neighborId in current.neighborSectorIds) {
                    if (visited.Contains(neighborId))
                        continue;

                    if (!MapData.Inst().sectors.ContainsKey(neighborId))
                        continue;

                    visited.Add(neighborId);
                    queue.Enqueue(neighborId);
                }
            }
        }
    }

    private void RemoveChunkSectors(int2 chunkCoord) {
        string chunkKey = ChunkKey(chunkCoord);

        if (!chunkSectorIds.TryGetValue(chunkKey, out List<string> sectorIds))
            return;

        foreach (string sectorId in sectorIds) {
            if (!MapData.Inst().sectors.TryGetValue(sectorId, out Sector sector))
                continue;

            foreach (string neighborId in sector.neighborSectorIds) {
                if (MapData.Inst().sectors.TryGetValue(neighborId, out Sector neighbor))
                    neighbor.neighborSectorIds.Remove(sectorId);
            }

            foreach (long linkHash in sector.linkHashes) {
                if (!linkHashToSectorIds.TryGetValue(linkHash, out HashSet<string> linkedSectors))
                    continue;

                linkedSectors.Remove(sectorId);

                if (linkedSectors.Count == 0)
                    linkHashToSectorIds.Remove(linkHash);
            }

            MapData.Inst().sectors.Remove(sectorId);
        }

        chunkSectorIds.Remove(chunkKey);
    }

    private void ClearAllTileSectorIds() {
        List<string> tileKeys = new(MapData.Inst().tiles.Keys);

        foreach (string tileKey in tileKeys) {
            Tile tile = MapData.Inst().tiles[tileKey];
            tile.sectorId = "";
            MapData.Inst().tiles[tileKey] = tile;
        }
    }

    private void ClearChunkTileSectorIds(int2 chunkCoord) {
        foreach (string tileKey in GetTileKeysInChunk(chunkCoord)) {
            Tile tile = MapData.Inst().tiles[tileKey];
            tile.sectorId = "";
            MapData.Inst().tiles[tileKey] = tile;
        }
    }

    private List<string> GetTileKeysInChunk(int2 chunkCoord) {
        List<string> result = new();
        List<string> tileKeys = new(MapData.Inst().tiles.Keys);

        foreach (string tileKey in tileKeys) {
            Tile tile = MapData.Inst().tiles[tileKey];

            if (GetChunkCoord(tile.position).Equals(chunkCoord))
                result.Add(tileKey);
        }

        return result;
    }

    private Dictionary<string, int2> GetAllChunkCoords() {
        Dictionary<string, int2> chunks = new();

        foreach (Tile tile in MapData.Inst().tiles.Values) {
            int2 chunkCoord = GetChunkCoord(tile.position);
            chunks[ChunkKey(chunkCoord)] = chunkCoord;
        }

        return chunks;
    }

    public bool AreTilesConnected(string aKey, string bKey) {
        if (!MapData.Inst().tiles.TryGetValue(aKey, out Tile a))
            return false;

        if (!MapData.Inst().tiles.TryGetValue(bKey, out Tile b))
            return false;

        if (string.IsNullOrEmpty(a.sectorId) || string.IsNullOrEmpty(b.sectorId))
            return false;

        if (!MapData.Inst().sectors.TryGetValue(a.sectorId, out Sector sectorA))
            return false;

        if (!MapData.Inst().sectors.TryGetValue(b.sectorId, out Sector sectorB))
            return false;

        return sectorA.connectivityId == sectorB.connectivityId;
    }

    private SectorPassability GetPassability(Tile tile) {
        if (tile.groundType == GroundType.Water)
            return SectorPassability.Impassable;

        return SectorPassability.Walkable;
    }

    public static int2 GetChunkCoord(int2 tilePosition) {
        return new int2(
            FloorDiv(tilePosition.x, ChunkSize),
            FloorDiv(tilePosition.y, ChunkSize)
        );
    }

    private static bool IsInsideChunk(int2 tilePosition, int2 chunkCoord) {
        return GetChunkCoord(tilePosition).Equals(chunkCoord);
    }

    private static int FloorDiv(int value, int divisor) {
        if (value >= 0)
            return value / divisor;

        return -((-value + divisor - 1) / divisor);
    }

    public static string TileKey(int2 position) {
        return $"{position.x}_{position.y}";
    }

    public static string ChunkKey(int2 chunkCoord) {
        return $"{chunkCoord.x}_{chunkCoord.y}";
    }

    public static string SectorId(int2 chunkCoord, int localIndex) {
        return $"{chunkCoord.x}_{chunkCoord.y}_{localIndex}";
    }

    public static long LinkHash(int2 a, int2 b) {
        if (ComparePositions(a, b) > 0) {
            int2 temp = a;
            a = b;
            b = temp;
        }

        unchecked {
            long hash = 17;
            hash = hash * 31 + a.x;
            hash = hash * 31 + a.y;
            hash = hash * 31 + b.x;
            hash = hash * 31 + b.y;
            return hash;
        }
    }

    private static int ComparePositions(int2 a, int2 b) {
        if (a.x != b.x)
            return a.x.CompareTo(b.x);

        return a.y.CompareTo(b.y);
    }
}