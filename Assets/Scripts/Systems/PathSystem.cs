using System.Collections.Generic;
using Unity.Mathematics;

public class PathSystem {

    public static PathSystem Inst() {
        return Game.Inst.system.pathSystem;
    }

    private static readonly int2[] Directions =
    {
        new int2(1, 0),
        new int2(-1, 0),
        new int2(0, 1),
        new int2(0, -1),

        new int2(1, 1),
        new int2(1, -1),
        new int2(-1, 1),
        new int2(-1, -1),
    };

    public Path FindPath(string startTileKey, string endTileKey) {
        if (!MapData.Inst().tiles.TryGetValue(startTileKey, out Tile startTile))
            return InvalidPath();

        if (!MapData.Inst().tiles.TryGetValue(endTileKey, out Tile endTile))
            return InvalidPath();

        return FindPath(startTile, endTile);
    }

    public Path FindPath(Tile startTile, Tile endTile) {
        if (!IsWalkable(startTile))
            return InvalidPath();

        if (!IsWalkable(endTile))
            return InvalidPath();

        if (string.IsNullOrEmpty(startTile.sectorId))
            return InvalidPath();

        if (string.IsNullOrEmpty(endTile.sectorId))
            return InvalidPath();

        if (!MapData.Inst().sectors.TryGetValue(startTile.sectorId, out Sector startSector))
            return InvalidPath();

        if (!MapData.Inst().sectors.TryGetValue(endTile.sectorId, out Sector endSector))
            return InvalidPath();

        if (!startSector.IsWalkable || !endSector.IsWalkable)
            return InvalidPath();

        if (startSector.connectivityId != endSector.connectivityId)
            return InvalidPath();

        HashSet<string> allowedSectorIds = FindSectorPath(
            startSector.id,
            endSector.id
        );

        if (allowedSectorIds.Count == 0)
            return InvalidPath();

        Queue<string> tileKeys = FindTilePath(
            startTile.key,
            endTile.key,
            allowedSectorIds
        );

        if (tileKeys.Count == 0)
            return InvalidPath();

        return CompletePath(tileKeys);
    }

    private HashSet<string> FindSectorPath(
        string startSectorId,
        string endSectorId
    ) {
        HashSet<string> result = new();

        if (startSectorId == endSectorId) {
            result.Add(startSectorId);
            return result;
        }

        MinHeap<SectorNode> openSet = new();
        HashSet<string> closedSet = new();

        Dictionary<string, string> cameFrom = new();
        Dictionary<string, int> gScore = new();

        gScore[startSectorId] = 0;

        openSet.Push(new SectorNode(startSectorId, 0));

        while (openSet.Count > 0) {
            SectorNode currentNode = openSet.Pop();
            string currentId = currentNode.sectorId;

            if (closedSet.Contains(currentId))
                continue;

            if (currentId == endSectorId)
                return ReconstructSectorSet(cameFrom, currentId);

            closedSet.Add(currentId);

            Sector currentSector = MapData.Inst().sectors[currentId];

            foreach (string neighborId in currentSector.neighborSectorIds) {
                if (closedSet.Contains(neighborId))
                    continue;

                if (!MapData.Inst().sectors.TryGetValue(neighborId, out Sector neighborSector))
                    continue;

                if (!neighborSector.IsWalkable)
                    continue;

                int tentativeG = gScore[currentId] + 10;

                if (!gScore.ContainsKey(neighborId) || tentativeG < gScore[neighborId]) {
                    cameFrom[neighborId] = currentId;
                    gScore[neighborId] = tentativeG;

                    int priority =
                        tentativeG +
                        SectorHeuristic(neighborSector, MapData.Inst().sectors[endSectorId]);

                    openSet.Push(new SectorNode(neighborId, priority));
                }
            }
        }

        return result;
    }

    private Queue<string> FindTilePath(
        string startTileKey,
        string endTileKey,
        HashSet<string> allowedSectorIds
    ) {
        Queue<string> emptyPath = new();

        MinHeap<TileNode> openSet = new();
        HashSet<string> closedSet = new();

        Dictionary<string, string> cameFrom = new();
        Dictionary<string, int> gScore = new();

        Tile startTile = MapData.Inst().tiles[startTileKey];
        Tile endTile = MapData.Inst().tiles[endTileKey];

        gScore[startTileKey] = 0;

        openSet.Push(new TileNode(
            startTileKey,
            Heuristic(startTile.position, endTile.position)
        ));

        while (openSet.Count > 0) {
            TileNode currentNode = openSet.Pop();
            string currentKey = currentNode.tileKey;

            if (closedSet.Contains(currentKey))
                continue;

            if (currentKey == endTileKey)
                return ReconstructTilePath(cameFrom, currentKey);

            closedSet.Add(currentKey);

            Tile currentTile = MapData.Inst().tiles[currentKey];

            foreach (int2 direction in Directions) {
                int2 neighborPosition = currentTile.position + direction;
                string neighborKey = TileKey(neighborPosition);

                if (!MapData.Inst().tiles.TryGetValue(neighborKey, out Tile neighbor))
                    continue;

                if (closedSet.Contains(neighborKey))
                    continue;

                if (!IsWalkable(neighbor))
                    continue;

                if (!allowedSectorIds.Contains(neighbor.sectorId))
                    continue;

                if (IsDiagonal(direction) && IsCornerBlocked(currentTile.position, direction))
                    continue;

                int movementCost = IsDiagonal(direction) ? 14 : 10;
                int tentativeG = gScore[currentKey] + movementCost;

                if (!gScore.ContainsKey(neighborKey) || tentativeG < gScore[neighborKey]) {
                    cameFrom[neighborKey] = currentKey;
                    gScore[neighborKey] = tentativeG;

                    int priority =
                        tentativeG +
                        Heuristic(neighbor.position, endTile.position);

                    openSet.Push(new TileNode(neighborKey, priority));
                }
            }
        }

        return emptyPath;
    }

    private HashSet<string> ReconstructSectorSet(
        Dictionary<string, string> cameFrom,
        string currentId
    ) {
        HashSet<string> result = new();

        result.Add(currentId);

        while (cameFrom.ContainsKey(currentId)) {
            currentId = cameFrom[currentId];
            result.Add(currentId);
        }

        return result;
    }

    private Queue<string> ReconstructTilePath(
        Dictionary<string, string> cameFrom,
        string currentKey
    ) {
        List<string> reversedPath = new();

        reversedPath.Add(currentKey);

        while (cameFrom.ContainsKey(currentKey)) {
            currentKey = cameFrom[currentKey];
            reversedPath.Add(currentKey);
        }

        reversedPath.Reverse();

        Queue<string> path = new();

        foreach (string tileKey in reversedPath) {
            path.Enqueue(tileKey);
        }

        return path;
    }

    private Path CompletePath(Queue<string> tileKeys) {
        return new Path {
            tile_keys = tileKeys,
            status = Path.PathStatus.Complete
        };
    }

    private Path InvalidPath() {
        return new Path {
            tile_keys = new Queue<string>(),
            status = Path.PathStatus.Invalid
        };
    }

    private bool IsWalkable(Tile tile) {
        return tile.groundType != GroundType.Water;
    }

    private bool IsDiagonal(int2 direction) {
        return direction.x != 0 && direction.y != 0;
    }

    private bool IsCornerBlocked(int2 position, int2 diagonalDirection) {
        int2 horizontalPosition = position + new int2(diagonalDirection.x, 0);
        int2 verticalPosition = position + new int2(0, diagonalDirection.y);

        string horizontalKey = TileKey(horizontalPosition);
        string verticalKey = TileKey(verticalPosition);

        if (!MapData.Inst().tiles.TryGetValue(horizontalKey, out Tile horizontalTile))
            return true;

        if (!MapData.Inst().tiles.TryGetValue(verticalKey, out Tile verticalTile))
            return true;

        return !IsWalkable(horizontalTile) || !IsWalkable(verticalTile);
    }

    private int Heuristic(int2 a, int2 b) {
        int dx = math.abs(a.x - b.x);
        int dy = math.abs(a.y - b.y);

        int diagonal = math.min(dx, dy);
        int straight = math.max(dx, dy) - diagonal;

        return diagonal * 14 + straight * 10;
    }

    private int SectorHeuristic(Sector a, Sector b) {
        int dx = math.abs(a.chunkCoord.x - b.chunkCoord.x);
        int dy = math.abs(a.chunkCoord.y - b.chunkCoord.y);

        int diagonal = math.min(dx, dy);
        int straight = math.max(dx, dy) - diagonal;

        return diagonal * 14 + straight * 10;
    }

    private static string TileKey(int2 position) {
        return $"{position.x}_{position.y}";
    }

    private struct TileNode {
        public string tileKey;
        public int priority;

        public TileNode(string tileKey, int priority) {
            this.tileKey = tileKey;
            this.priority = priority;
        }
    }

    private struct SectorNode {
        public string sectorId;
        public int priority;

        public SectorNode(string sectorId, int priority) {
            this.sectorId = sectorId;
            this.priority = priority;
        }
    }

    private class MinHeap<T> {
        private readonly List<T> items = new();

        public int Count => items.Count;

        public void Push(T item) {
            items.Add(item);
            BubbleUp(items.Count - 1);
        }

        public T Pop() {
            T result = items[0];

            int lastIndex = items.Count - 1;
            items[0] = items[lastIndex];
            items.RemoveAt(lastIndex);

            if (items.Count > 0)
                BubbleDown(0);

            return result;
        }

        private void BubbleUp(int index) {
            while (index > 0) {
                int parentIndex = (index - 1) / 2;

                if (Compare(items[index], items[parentIndex]) >= 0)
                    break;

                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void BubbleDown(int index) {
            while (true) {
                int leftIndex = index * 2 + 1;
                int rightIndex = index * 2 + 2;
                int smallestIndex = index;

                if (leftIndex < items.Count && Compare(items[leftIndex], items[smallestIndex]) < 0)
                    smallestIndex = leftIndex;

                if (rightIndex < items.Count && Compare(items[rightIndex], items[smallestIndex]) < 0)
                    smallestIndex = rightIndex;

                if (smallestIndex == index)
                    break;

                Swap(index, smallestIndex);
                index = smallestIndex;
            }
        }

        private void Swap(int a, int b) {
            T temp = items[a];
            items[a] = items[b];
            items[b] = temp;
        }

        private int Compare(T a, T b) {
            return GetPriority(a).CompareTo(GetPriority(b));
        }

        private int GetPriority(T item) {
            if (item is TileNode tileNode)
                return tileNode.priority;

            if (item is SectorNode sectorNode)
                return sectorNode.priority;

            return 0;
        }
    }
}