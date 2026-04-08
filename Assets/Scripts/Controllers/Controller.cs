using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Controller {


    public void DealWithInput() {


        if (Keyboard.current.qKey.wasPressedThisFrame) {
            Debug.Log("Q key was pressed.");
            JobManager.Inst().InterruptAllJobs();
        }

    }



}
