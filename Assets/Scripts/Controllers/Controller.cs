using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Controller {

    public static Controller Inst;

    public SaveController saveController = new();

    public void DealWithInput() {

        saveController.DealWithInput();

    }



}
