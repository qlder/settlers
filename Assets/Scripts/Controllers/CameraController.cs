using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController {

    public static CameraController Inst() {
        return Controller.Inst.cameraController;
    }

    public void DealWithInput(float deltaTime) {

        float2 moveAmount = float2.zero;
        float moveSpeed = 10f * deltaTime;

        if (Keyboard.current.wKey.isPressed) {
            moveAmount.y += moveSpeed;
        }

        if (Keyboard.current.sKey.isPressed) {
            moveAmount.y -= moveSpeed;
        }

        if (Keyboard.current.aKey.isPressed) {
            moveAmount.x -= moveSpeed;
        }

        if (Keyboard.current.dKey.isPressed) {
            moveAmount.x += moveSpeed;
        }

        float zoomAmount = 0f;
        float zoomSpeed = 2f * deltaTime;

        if (Keyboard.current.qKey.isPressed) {
            zoomAmount += zoomSpeed;
        }
        if (Keyboard.current.eKey.isPressed) {
            zoomAmount -= zoomSpeed;
        }

        CameraData.Inst().position += moveAmount;
        float zoom = CameraData.Inst().zoom + zoomAmount;
        zoom = Mathf.Clamp01(zoom);
        CameraData.Inst().zoom = zoom;
    }

}