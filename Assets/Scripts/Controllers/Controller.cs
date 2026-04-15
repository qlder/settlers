using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Controller {


    public void DealWithInput() {


        if (Keyboard.current.qKey.wasPressedThisFrame) {
            Debug.Log("Q key was pressed.");
            JobManager.Inst().InterruptAllJobs();
        }

        if (Keyboard.current.pKey.wasPressedThisFrame) {
            Debug.Log("P key was pressed.");
            Game.Inst.data.Save("savefile.json");
        }

        if (Keyboard.current.lKey.wasPressedThisFrame) {
            Debug.Log("L key was pressed.");
            GameData.Load("savefile.json");
        }

    }



}
