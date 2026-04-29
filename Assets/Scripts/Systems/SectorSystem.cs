using System.Collections.Generic;
using Unity.Mathematics;

public class SectorSystem {
    public const int ChunkSize = 16;

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
        MapData.Inst().ClearSectorData();
        ClearAllTileSectorIds();

        Dictionary<string, int2> chunks = GetAllChunkCoords();

        foreach (int2 chunkCoord in chunks.Values) {
            BuildChunkSectors(chunkCoord);
        }

        RebuildAllSectorLinks();
        RebuildConnectivityIds();
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

        if (!MapData.Inst().chunkSectorIds.ContainsKey(chunkKey))
            MapData.Inst().chunkSectorIds[chunkKey] = new List<int>();

        HashSet<string> visited = new();

        foreach (string tileKey in GetTileKeysInChunk(chunkCoord)) {
            if (visited.Contains(tileKey))
                continue;

            Tile tile = MapData.Inst().tiles[tileKey];

            if (!IsSectorable(tile)) {
                visited.Add(tileKey);
                continue;
            }

            Sector sector = FloodFillSectorInsideChunk(tileKey, chunkCoord, visited);

            MapData.Inst().sectors[sector.id] = sector;
            MapData.Inst().chunkSectorIds[chunkKey].Add(sector.id);
        }
    }

    private Sector FloodFillSectorInsideChunk(
        string startTileKey,
        int2 chunkCoord,
        HashSet<string> visited
    ) {
        Tile startTile = MapData.Inst().tiles[startTileKey];

        Sector sector = new() {
            id = MapData.Inst().GetNextSectorId(),
            connectivityId = 0,
            chunkCoord = chunkCoord,
            passability = GetPassability(startTile)
        };

        Queue<string> queue = new();
        queue.Enqueue(startTileKey);
        visited.Add(startTileKey);

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

                if (!CanBeInSameSector(tile, neighbor))
                    continue;

                visited.Add(neighborKey);
                queue.Enqueue(neighborKey);
            }
        }

        return sector;
    }

    private void RebuildAllSectorLinks() {
        MapData.Inst().linkHashToSectorIds.Clear();

        foreach (Sector sector in MapData.Inst().sectors.Values) {
            sector.neighborSectorIds.Clear();
            sector.linkHashes.Clear();
        }

        List<string> tileKeys = new(MapData.Inst().tiles.Keys);

        foreach (string tileKey in tileKeys) {
            Tile tile = MapData.Inst().tiles[tileKey];

            if (tile.sectorId == 0)
                continue;

            foreach (int2 direction in LinkDirections) {
                int2 neighborPosition = tile.position + direction;
                string neighborKey = TileKey(neighborPosition);

                if (!MapData.Inst().tiles.TryGetValue(neighborKey, out Tile neighbor))
                    continue;

                if (neighbor.sectorId == 0)
                    continue;

                if (neighbor.sectorId == tile.sectorId)
                    continue;

                Sector a = MapData.Inst().sectors[tile.sectorId];
                Sector b = MapData.Inst().sectors[neighbor.sectorId];

                if (!CanSectorsConnect(a, b))
                    continue;

                long linkHash = LinkHash(tile.position, neighbor.position);

                RegisterSectorLink(a.id, linkHash);
                RegisterSectorLink(b.id, linkHash);
            }
        }
    }

    private void RegisterSectorLink(int sectorId, long linkHash) {
        if (!MapData.Inst().sectors.TryGetValue(sectorId, out Sector sector))
            return;

        sector.linkHashes.Add(linkHash);

        if (!MapData.Inst().linkHashToSectorIds.TryGetValue(linkHash, out HashSet<int> linkedSectorIds)) {
            linkedSectorIds = new HashSet<int>();
            MapData.Inst().linkHashToSectorIds[linkHash] = linkedSectorIds;
        }

        foreach (int otherSectorId in linkedSectorIds) {
            if (otherSectorId == sectorId)
                continue;

            sector.neighborSectorIds.Add(otherSectorId);

            if (MapData.Inst().sectors.TryGetValue(otherSectorId, out Sector otherSector))
                otherSector.neighborSectorIds.Add(sectorId);
        }

        linkedSectorIds.Add(sectorId);
    }

    private void RebuildConnectivityIds() {
        MapData.Inst().ResetConnectivityIds();

        HashSet<int> visited = new();
        List<int> sectorIds = new(MapData.Inst().sectors.Keys);

        foreach (int sectorId in sectorIds) {
            if (visited.Contains(sectorId))
                continue;

            int connectivityId = MapData.Inst().GetNextConnectivityId();

            Queue<int> queue = new();
            queue.Enqueue(sectorId);
            visited.Add(sectorId);

            while (queue.Count > 0) {
                int currentId = queue.Dequeue();

                Sector current = MapData.Inst().sectors[currentId];
                current.connectivityId = connectivityId;

                foreach (int neighborId in current.neighborSectorIds) {
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

        if (!MapData.Inst().chunkSectorIds.TryGetValue(chunkKey, out List<int> sectorIds))
            return;

        foreach (int sectorId in sectorIds) {
            if (!MapData.Inst().sectors.TryGetValue(sectorId, out Sector sector))
                continue;

            foreach (int neighborId in sector.neighborSectorIds) {
                if (MapData.Inst().sectors.TryGetValue(neighborId, out Sector neighbor))
                    neighbor.neighborSectorIds.Remove(sectorId);
            }

            foreach (long linkHash in sector.linkHashes) {
                if (!MapData.Inst().linkHashToSectorIds.TryGetValue(linkHash, out HashSet<int> linkedSectors))
                    continue;

                linkedSectors.Remove(sectorId);

                if (linkedSectors.Count == 0)
                    MapData.Inst().linkHashToSectorIds.Remove(linkHash);
            }

            MapData.Inst().sectors.Remove(sectorId);
        }

        MapData.Inst().chunkSectorIds.Remove(chunkKey);
    }

    private void ClearAllTileSectorIds() {
        List<string> tileKeys = new(MapData.Inst().tiles.Keys);

        foreach (string tileKey in tileKeys) {
            Tile tile = MapData.Inst().tiles[tileKey];
            tile.sectorId = 0;
            MapData.Inst().tiles[tileKey] = tile;
        }
    }

    private void ClearChunkTileSectorIds(int2 chunkCoord) {
        foreach (string tileKey in GetTileKeysInChunk(chunkCoord)) {
            Tile tile = MapData.Inst().tiles[tileKey];
            tile.sectorId = 0;
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

        if (a.sectorId == 0 || b.sectorId == 0)
            return false;

        if (!MapData.Inst().sectors.TryGetValue(a.sectorId, out Sector sectorA))
            return false;

        if (!MapData.Inst().sectors.TryGetValue(b.sectorId, out Sector sectorB))
            return false;

        return sectorA.connectivityId == sectorB.connectivityId;
    }

    public bool AreSectorsConnected(int sectorAId, int sectorBId) {
        if (!MapData.Inst().sectors.TryGetValue(sectorAId, out Sector a))
            return false;

        if (!MapData.Inst().sectors.TryGetValue(sectorBId, out Sector b))
            return false;

        return a.connectivityId == b.connectivityId;
    }

    private bool IsSectorable(Tile tile) {
        return GetPassability(tile) != SectorPassability.Impassable;
    }

    private bool CanBeInSameSector(Tile a, Tile b) {
        if (!IsSectorable(a) || !IsSectorable(b))
            return false;

        return GetPassability(a) == GetPassability(b);
    }

    private bool CanSectorsConnect(Sector a, Sector b) {
        return a.passability == b.passability;
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