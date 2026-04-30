using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Controller {

    public static Controller Inst;

    public SaveController saveController = new();
    public CameraController cameraController = new();

    public void DealWithInput(float deltaTime) {

        cameraController.DealWithInput(deltaTime);
        saveController.DealWithInput(deltaTime);


    }



}
