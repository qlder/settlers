using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public interface ILiving {

    public Sex sex { get; set; }
    public float2? position { get; set; }

}
