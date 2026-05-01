using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;

public struct Job : IModel {

    public int Id { get; set; }
    #region GetSet
    public static Job? Get(int id) {
        //try to get or return null
        if (!JobData.Inst().Jobs.TryGetValue(id, out var job)) {
            return null;
        }
        return job;
    }

    public void Save() {
        JobData.Inst().Jobs[Id] = this;
    }
    #endregion

    public string Name;



}

