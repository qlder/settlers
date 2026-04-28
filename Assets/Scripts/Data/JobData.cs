using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Newtonsoft.Json;


[System.Serializable]
public class JobData
{

    public Dictionary<string, Tile> tiles = new();

    public static JobData Inst()
    {
        return Game.Inst.data.jobData;
    }

}
