using System.Collections.Generic;
using Unity.Mathematics;

public enum SectorPassability {
    Impassable,
    Walkable
}

public class Sector {
    // Example: "0_0_0"
    public string Key;

    // Example: "c_0"
    public string ConnectivityId;

    public int2 chunkCoord;
    public SectorPassability Passability;

    public List<string> tileKeys = new();
    public HashSet<string> neighborSectorIds = new();
    public HashSet<long> linkHashes = new();

    public bool IsWalkable =>
        Passability == SectorPassability.Walkable;
}