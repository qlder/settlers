using System.Collections.Generic;
using Unity.Mathematics;

public enum SectorPassability {
    Impassable,
    Walkable
}
[System.Serializable]
public class Sector {
    // Example: "0_0_0"
    public string id;

    // Example: "c_0"
    public string connectivityId;

    public int2 chunkCoord;
    public SectorPassability passability;

    public List<string> tileKeys = new();
    public HashSet<string> neighborSectorIds = new();
    public HashSet<long> linkHashes = new();

    public bool IsWalkable =>
        passability == SectorPassability.Walkable;
}