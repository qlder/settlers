using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;

public struct Entity : IModel, ILiving {

    #region GetSet
    public static Entity? Get(long id) {
        //try to get or return null
        if (!LivingData.Inst().Entities.TryGetValue(id, out var entity)) {
            return null;
        }
        return entity;
    }

    public void Save() {
        LivingData.Inst().Entities[Id] = this;
    }
    #endregion

    public long Id { get; set; }
    public string Name;

    public SpeciesType species { get; set; }
    public Sex sex { get; set; }
    public float2? position { get; set; }


}

