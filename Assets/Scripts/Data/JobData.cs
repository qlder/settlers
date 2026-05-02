using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Newtonsoft.Json;


[System.Serializable]
public class JobData {

    public static JobData Inst() {
        return Game.Inst.data.jobData;
    }

    public int nextId = 1;
    public int GetNextId() {
        return nextId++;
    }

    public Dictionary<int, Job> Jobs = new();


}
