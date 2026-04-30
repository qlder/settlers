using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Newtonsoft.Json;


[System.Serializable]
public class CameraData {

    public static CameraData Inst() {
        return Game.Inst.data.cameraData;
    }

    public float2 position = new float2(0f, 0f);
    public float zoom = 1f;

}
