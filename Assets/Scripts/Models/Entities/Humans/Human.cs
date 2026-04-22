using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;

public struct Human : IModel, ILiving {

    public static Human Get(long id) {
        return Game.Inst.data.humanData.Humans[id];
    }

    public long Id { get; set; }
    public string Name;
    public Sex sex { get; set; }
    public float2? position { get; set; }

    public Hair hairStyle;

}
