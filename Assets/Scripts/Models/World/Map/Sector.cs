using System.Collections.Generic;
using Unity.Mathematics;

public enum SectorPassability {
    Impassable,
    Walkable
}

[System.Serializable]
public class Sector {
    public int id;
    public int connectivityId;

    public int2 chunkCoord;
    public SectorPassability passability;

    public List<string> tileKeys = new();
    public HashSet<int> neighborSectorIds = new();
    public HashSet<long> linkHashes = new();

    public bool IsWalkable => passability == SectorPassability.Walkable;
}