using Unity.Mathematics;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using SQLite;


public class Tile {

    [PrimaryKey]
    public string Key { get; set; }

    [Indexed]
    public int X { get; set; }
    [Indexed]
    public int Y { get; set; }

    public GroundType GroundType { get; set; }
    public string SectorKey { get; set; }

    public int2 GetPosition() => new int2(X, Y);

}
