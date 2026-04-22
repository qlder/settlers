using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Xml.Serialization;

public class GameSystem {

    HumanSystem humanSystem = new();

    public static GameSystem Inst() {
        return Game.Inst.system;
    }

    public void Tick(int ticks) {
        humanSystem.Tick(ticks);
    }

}
