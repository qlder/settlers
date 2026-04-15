using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TargetFinding {
    static TargetFinding inst;
    public PathFindingThreader pathFindingThreader { get; private set; }

    public static TargetFinding Inst() {
        return Game.Inst.targetFinding;
    }

    public void Initialize() {
        pathFindingThreader = new PathFindingThreader();
        pathFindingThreader.Start();
    }

    public void Shutdown() {
        pathFindingThreader.Stop();
        pathFindingThreader = null;
    }

    public PathRequest FindPath(Tile start, Tile target) {
        if (start == null || target == null) {
            Tile fallback = Tile.Get(0, 0);

            PathRequest failedRequest = new PathRequest(
                start ?? fallback,
                target ?? fallback
            );

            failedRequest.Status = PathRequestStatus.Failed;
            failedRequest.Path = null;
            return failedRequest;
        }

        PathRequest request = new PathRequest(start, target);
        pathFindingThreader.Enqueue(request);
        // UnityEngine.Debug.Log($"Enqueued pathfinding request from ({start.X}, {start.Z}) to ({target.X}, {target.Z})");
        return request;
    }

    internal void ProcessRequest(PathRequest request) {
        request.Status = PathRequestStatus.Running;

        Queue<Tile> path = FindPathInternal(
            request.StartX,
            request.StartZ,
            request.TargetX,
            request.TargetZ
        );

        if (path != null && path.Count > 0) {
            request.Path = path;
            request.Status = PathRequestStatus.Complete;
        } else {
            request.Path = null;
            request.Status = PathRequestStatus.Failed;
        }
    }

    Queue<Tile> FindPathInternal(int startX, int startZ, int targetX, int targetZ) {
        Map map = Map.Inst();

        if (!IsInBounds(startX, startZ, map) || !IsInBounds(targetX, targetZ, map)) {
            return null;
        }

        if (startX == targetX && startZ == targetZ) {
            Queue<Tile> singleTilePath = new Queue<Tile>();
            singleTilePath.Enqueue(map.tiles[startX, startZ]);
            return singleTilePath;
        }

        PathNode[,] nodes = new PathNode[map.tileLength, map.tileLength];
        bool[,] closedSet = new bool[map.tileLength, map.tileLength];
        List<PathNode> openList = new List<PathNode>();

        PathNode startNode = GetNode(startX, startZ, nodes);
        PathNode targetNode = GetNode(targetX, targetZ, nodes);

        startNode.GCost = 0;
        startNode.HCost = GetDistance(startX, startZ, targetX, targetZ);

        openList.Add(startNode);

        while (openList.Count > 0) {
            PathNode currentNode = GetLowestCostNode(openList);

            if (currentNode.X == targetNode.X && currentNode.Z == targetNode.Z) {
                return RetracePath(startNode, currentNode, map);
            }

            openList.Remove(currentNode);
            closedSet[currentNode.X, currentNode.Z] = true;

            foreach (PathNode neighbor in GetNeighbors(currentNode, nodes, map)) {
                if (closedSet[neighbor.X, neighbor.Z]) {
                    continue;
                }

                if (!IsWalkable(neighbor.X, neighbor.Z, map) &&
                    !(neighbor.X == targetX && neighbor.Z == targetZ)) {
                    continue;
                }

                int movementCostToNeighbor = currentNode.GCost + GetDistance(
                    currentNode.X,
                    currentNode.Z,
                    neighbor.X,
                    neighbor.Z
                );

                if (movementCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor)) {
                    neighbor.GCost = movementCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor.X, neighbor.Z, targetX, targetZ);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor)) {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    PathNode GetNode(int x, int z, PathNode[,] nodes) {
        if (nodes[x, z] == null) {
            nodes[x, z] = new PathNode(x, z);
        }

        return nodes[x, z];
    }

    PathNode GetLowestCostNode(List<PathNode> openList) {
        PathNode bestNode = openList[0];

        for (int i = 1; i < openList.Count; i++) {
            PathNode node = openList[i];

            if (node.FCost < bestNode.FCost ||
                (node.FCost == bestNode.FCost && node.HCost < bestNode.HCost)) {
                bestNode = node;
            }
        }

        return bestNode;
    }

    List<PathNode> GetNeighbors(PathNode node, PathNode[,] nodes, Map map) {
        List<PathNode> neighbors = new List<PathNode>();

        for (int dx = -1; dx <= 1; dx++) {
            for (int dz = -1; dz <= 1; dz++) {
                if (dx == 0 && dz == 0) {
                    continue;
                }

                int neighborX = node.X + dx;
                int neighborZ = node.Z + dz;

                if (!IsInBounds(neighborX, neighborZ, map)) {
                    continue;
                }

                neighbors.Add(GetNode(neighborX, neighborZ, nodes));
            }
        }

        return neighbors;
    }

    Queue<Tile> RetracePath(PathNode startNode, PathNode endNode, Map map) {
        List<Tile> reversed = new List<Tile>();
        PathNode currentNode = endNode;

        while (currentNode != null) {
            reversed.Add(map.tiles[currentNode.X, currentNode.Z]);

            if (currentNode.X == startNode.X && currentNode.Z == startNode.Z) {
                break;
            }

            currentNode = currentNode.Parent;
        }

        reversed.Reverse();

        Queue<Tile> pathQueue = new Queue<Tile>(reversed);
        return pathQueue;
    }

    bool IsInBounds(int x, int z, Map map) {
        return x >= 0 && x < map.tileLength && z >= 0 && z < map.tileLength;
    }

    bool IsWalkable(int x, int z, Map map) {
        Tile tile = map.tiles[x, z];
        return tile != null && tile.isWalkable;
    }

    int GetDistance(int x1, int z1, int x2, int z2) {
        int dstX = Math.Abs(x1 - x2);
        int dstZ = Math.Abs(z1 - z2);

        if (dstX > dstZ) {
            return 14 * dstZ + 10 * (dstX - dstZ);
        }

        return 14 * dstX + 10 * (dstZ - dstX);
    }
}