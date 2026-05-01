using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;

[System.Serializable]
public struct Path : IModel {

    public long Id { get; set; }

    #region GetSet
    public static Path? Get(long id) {
        //try to get or return null
        if (!LivingData.Inst().Paths.TryGetValue(id, out var path)) {
            return null;
        }
        return path;
    }

    public void Save() {
        LivingData.Inst().Paths[Id] = this;
    }
    #endregion

    public enum PathStatus {
        Awaiting = 0,
        Complete = 1,
        Invalid = 2
    }

    public Queue<float2> points;
    public PathStatus status;

}

