using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraRenderer : MonoBehaviour {


    static public CameraRenderer Inst { get; private set; }

    private Camera cam;

    void OnEnable() {
        this.cam = this.GetComponentInChildren<Camera>();
    }

    private void Update() {
        if (Game.Inst == null || Game.Inst.data == null || Game.Inst.data.cameraData == null)
            return;

        this.transform.position = new Vector3(Game.Inst.data.cameraData.position.x, 0f, Game.Inst.data.cameraData.position.y);
        this.cam.orthographicSize = 5f + (25f * Game.Inst.data.cameraData.zoom);
    }


}