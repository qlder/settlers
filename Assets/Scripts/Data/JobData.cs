using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Newtonsoft.Json;


[System.Serializable]
public class JobData {

    public static JobData Inst() {
        return Game.Inst.data.jobData;
    }

    public Dictionary<long, Job> Jobs = new();

}
