using System.Collections.Generic;
using UnityEngine;

public class PathFinding {
    public static PathFinding Inst() {
        return Game.Inst.pathFinding;
    }

    public Queue<Tile> GetPathForEntity(Entity entity, Tile target) {
        return AStarPathFinding(entity, target);
    }

    public Queue<Tile> AStarPathFinding(Entity entity, Tile target) {
        Tile start = entity.GetTile();

        if (start == null || target == null || start == target)
            return new Queue<Tile>();

        Map map = Map.Inst();

        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();

        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Dictionary<Tile, int> gScore = new Dictionary<Tile, int>();
        Dictionary<Tile, int> fScore = new Dictionary<Tile, int>();

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, target);

        while (openSet.Count > 0) {
            Tile current = GetLowestFScore(openSet, fScore);

            if (current == target)
                return ReconstructPath(cameFrom, current, start);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Tile neighbor in GetNeighbors(map, current, entity, target)) {
                if (neighbor == null || closedSet.Contains(neighbor))
                    continue;

                int tentativeGScore = GetScore(gScore, current) + 1;

                if (!openSet.Contains(neighbor)) {
                    openSet.Add(neighbor);
                } else if (tentativeGScore >= GetScore(gScore, neighbor)) {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = tentativeGScore + Heuristic(neighbor, target);
            }
        }

        return new Queue<Tile>();
    }

    // Correct for 8-direction movement with uniform move cost = 1.
    private int Heuristic(Tile a, Tile b) {
        return Mathf.Max(Mathf.Abs(a.X - b.X), Mathf.Abs(a.Z - b.Z));
    }

    private int GetScore(Dictionary<Tile, int> scores, Tile tile) {
        return scores.TryGetValue(tile, out int score) ? score : int.MaxValue;
    }

    private Tile GetLowestFScore(List<Tile> openSet, Dictionary<Tile, int> fScore) {
        Tile bestTile = openSet[0];
        int bestScore = GetScore(fScore, bestTile);

        for (int i = 1; i < openSet.Count; i++) {
            Tile tile = openSet[i];
            int score = GetScore(fScore, tile);

            if (score < bestScore) {
                bestScore = score;
                bestTile = tile;
            }
        }

        return bestTile;
    }

    private IEnumerable<Tile> GetNeighbors(Map map, Tile tile, Entity entity, Tile target) {
        int x = tile.X;
        int z = tile.Z;

        for (int dx = -1; dx <= 1; dx++) {
            for (int dz = -1; dz <= 1; dz++) {
                if (dx == 0 && dz == 0)
                    continue;

                int nx = x + dx;
                int nz = z + dz;

                if (nx < 0 || nx >= map.tileLength || nz < 0 || nz >= map.tileLength)
                    continue;

                Tile neighbor = map.tiles[nx, nz];

                if (!CanEntityMoveToTile(entity, neighbor, target))
                    continue;

                // Prevent diagonal corner cutting.
                if (dx != 0 && dz != 0) {
                    Tile sideA = map.tiles[x + dx, z];
                    Tile sideB = map.tiles[x, z + dz];

                    if (!CanEntityMoveToTile(entity, sideA, target) ||
                        !CanEntityMoveToTile(entity, sideB, target)) {
                        continue;
                    }
                }

                yield return neighbor;
            }
        }
    }

    private bool CanEntityMoveToTile(Entity entity, Tile tile, Tile target) {
        if (tile == null)
            return false;

        if (tile == target)
            return true;

        // Replace with your actual logic.
        // Example:
        // return !tile.IsBlocked && tile.occupier == null;

        return true;
    }

    private Queue<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile current, Tile start) {
        List<Tile> path = new List<Tile>();

        while (current != null) {
            path.Add(current);

            if (!cameFrom.TryGetValue(current, out current))
                break;
        }

        path.Reverse();

        if (path.Count > 0 && path[0] == start)
            path.RemoveAt(0);

        return new Queue<Tile>(path);
    }
}