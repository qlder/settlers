using System.Collections.Generic;
using Unity.Mathematics;

public class PathSystem
{

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

    public Queue<Tile> FindPath(Tile startTile, Tile endTile)
    {
        Queue<Tile> emptyPath = new();

        if (!IsWalkable(startTile))
            return emptyPath;

        if (!IsWalkable(endTile))
            return emptyPath;

        if (string.IsNullOrEmpty(startTile.sectorId))
            return emptyPath;

        if (string.IsNullOrEmpty(endTile.sectorId))
            return emptyPath;

        if (!MapData.Inst().sectors.TryGetValue(startTile.sectorId, out Sector startSector))
            return emptyPath;

        if (!MapData.Inst().sectors.TryGetValue(endTile.sectorId, out Sector endSector))
            return emptyPath;

        if (startSector.connectivityId != endSector.connectivityId)
            return emptyPath;

        HashSet<string> allowedSectorIds = FindSectorPath(
            startSector.id,
            endSector.id
        );

        if (allowedSectorIds.Count == 0)
            return emptyPath;

        return FindTilePath(
            startTile.key,
            endTile.key,
            allowedSectorIds
        );
    }

    public Queue<Tile> FindPath(string startTileKey, string endTileKey)
    {
        if (!MapData.Inst().tiles.TryGetValue(startTileKey, out Tile startTile))
            return new Queue<Tile>();

        if (!MapData.Inst().tiles.TryGetValue(endTileKey, out Tile endTile))
            return new Queue<Tile>();

        return FindPath(startTile, endTile);
    }

    private HashSet<string> FindSectorPath(
        string startSectorId,
        string endSectorId
    )
    {
        HashSet<string> result = new();

        if (startSectorId == endSectorId)
        {
            result.Add(startSectorId);
            return result;
        }

        MinHeap<SectorNode> openSet = new();
        HashSet<string> closedSet = new();

        Dictionary<string, string> cameFrom = new();
        Dictionary<string, int> gScore = new();

        gScore[startSectorId] = 0;

        openSet.Push(new SectorNode(
            startSectorId,
            0
        ));

        while (openSet.Count > 0)
        {
            SectorNode currentNode = openSet.Pop();
            string currentId = currentNode.sectorId;

            if (closedSet.Contains(currentId))
                continue;

            if (currentId == endSectorId)
                return ReconstructSectorSet(cameFrom, currentId);

            closedSet.Add(currentId);

            Sector currentSector = MapData.Inst().sectors[currentId];

            foreach (string neighborId in currentSector.neighborSectorIds)
            {
                if (closedSet.Contains(neighborId))
                    continue;

                if (!MapData.Inst().sectors.TryGetValue(neighborId, out Sector neighborSector))
                    continue;

                if (!neighborSector.IsWalkable)
                    continue;

                int tentativeG = gScore[currentId] + 1;

                if (!gScore.ContainsKey(neighborId) || tentativeG < gScore[neighborId])
                {
                    cameFrom[neighborId] = currentId;
                    gScore[neighborId] = tentativeG;

                    int priority = tentativeG + SectorHeuristic(neighborSector, MapData.Inst().sectors[endSectorId]);

                    openSet.Push(new SectorNode(
                        neighborId,
                        priority
                    ));
                }
            }
        }

        return result;
    }

    private Queue<Tile> FindTilePath(
        string startTileKey,
        string endTileKey,
        HashSet<string> allowedSectorIds
    )
    {
        Queue<Tile> emptyPath = new();

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

        while (openSet.Count > 0)
        {
            TileNode currentNode = openSet.Pop();
            string currentKey = currentNode.tileKey;

            if (closedSet.Contains(currentKey))
                continue;

            if (currentKey == endTileKey)
                return ReconstructTilePath(cameFrom, currentKey);

            closedSet.Add(currentKey);

            Tile currentTile = MapData.Inst().tiles[currentKey];

            foreach (int2 direction in Directions)
            {
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

                if (!gScore.ContainsKey(neighborKey) || tentativeG < gScore[neighborKey])
                {
                    cameFrom[neighborKey] = currentKey;
                    gScore[neighborKey] = tentativeG;

                    int priority = tentativeG + Heuristic(neighbor.position, endTile.position);

                    openSet.Push(new TileNode(
                        neighborKey,
                        priority
                    ));
                }
            }
        }

        return emptyPath;
    }

    private HashSet<string> ReconstructSectorSet(
        Dictionary<string, string> cameFrom,
        string currentId
    )
    {
        HashSet<string> result = new();

        result.Add(currentId);

        while (cameFrom.ContainsKey(currentId))
        {
            currentId = cameFrom[currentId];
            result.Add(currentId);
        }

        return result;
    }

    private Queue<Tile> ReconstructTilePath(
        Dictionary<string, string> cameFrom,
        string currentKey
    )
    {
        List<Tile> reversedPath = new();

        reversedPath.Add(MapData.Inst().tiles[currentKey]);

        while (cameFrom.ContainsKey(currentKey))
        {
            currentKey = cameFrom[currentKey];
            reversedPath.Add(MapData.Inst().tiles[currentKey]);
        }

        reversedPath.Reverse();

        Queue<Tile> path = new();

        foreach (Tile tile in reversedPath)
        {
            path.Enqueue(tile);
        }

        return path;
    }

    private bool IsWalkable(Tile tile)
    {
        return tile.groundType != GroundType.Water;
    }

    private bool IsDiagonal(int2 direction)
    {
        return direction.x != 0 && direction.y != 0;
    }

    private bool IsCornerBlocked(int2 position, int2 diagonalDirection)
    {
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

    private int Heuristic(int2 a, int2 b)
    {
        int dx = math.abs(a.x - b.x);
        int dy = math.abs(a.y - b.y);

        int diagonal = math.min(dx, dy);
        int straight = math.max(dx, dy) - diagonal;

        return diagonal * 14 + straight * 10;
    }

    private int SectorHeuristic(Sector a, Sector b)
    {
        int dx = math.abs(a.chunkCoord.x - b.chunkCoord.x);
        int dy = math.abs(a.chunkCoord.y - b.chunkCoord.y);

        return dx + dy;
    }

    private static string TileKey(int2 position)
    {
        return $"{position.x}_{position.y}";
    }

    private struct TileNode
    {
        public string tileKey;
        public int priority;

        public TileNode(string tileKey, int priority)
        {
            this.tileKey = tileKey;
            this.priority = priority;
        }
    }

    private struct SectorNode
    {
        public string sectorId;
        public int priority;

        public SectorNode(string sectorId, int priority)
        {
            this.sectorId = sectorId;
            this.priority = priority;
        }
    }

    private class MinHeap<T>
    {
        private readonly List<T> items = new();
        private readonly IComparer<T> comparer;

        public int Count => items.Count;

        public MinHeap()
        {
            comparer = Comparer<T>.Create(Compare);
        }

        public void Push(T item)
        {
            items.Add(item);
            BubbleUp(items.Count - 1);
        }

        public T Pop()
        {
            T result = items[0];

            int lastIndex = items.Count - 1;
            items[0] = items[lastIndex];
            items.RemoveAt(lastIndex);

            if (items.Count > 0)
                BubbleDown(0);

            return result;
        }

        private void BubbleUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;

                if (comparer.Compare(items[index], items[parentIndex]) >= 0)
                    break;

                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void BubbleDown(int index)
        {
            while (true)
            {
                int leftIndex = index * 2 + 1;
                int rightIndex = index * 2 + 2;
                int smallestIndex = index;

                if (
                    leftIndex < items.Count &&
                    comparer.Compare(items[leftIndex], items[smallestIndex]) < 0
                )
                {
                    smallestIndex = leftIndex;
                }

                if (
                    rightIndex < items.Count &&
                    comparer.Compare(items[rightIndex], items[smallestIndex]) < 0
                )
                {
                    smallestIndex = rightIndex;
                }

                if (smallestIndex == index)
                    break;

                Swap(index, smallestIndex);
                index = smallestIndex;
            }
        }

        private void Swap(int a, int b)
        {
            T temp = items[a];
            items[a] = items[b];
            items[b] = temp;
        }

        private int Compare(T a, T b)
        {
            int aPriority = GetPriority(a);
            int bPriority = GetPriority(b);

            return aPriority.CompareTo(bPriority);
        }

        private int GetPriority(T item)
        {
            if (item is TileNode tileNode)
                return tileNode.priority;

            if (item is SectorNode sectorNode)
                return sectorNode.priority;

            return 0;
        }
    }
}