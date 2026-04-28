using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;

public struct Job : IModel {

    #region GetSet
    public static Job? Get(long id) {
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

    public long Id { get; set; }
    public string Name;



}

