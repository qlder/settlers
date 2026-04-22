using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class GameData {

    public static GameData Inst() {
        return Game.Inst.data;
    }

    public long nextId = 1;

    public GameOptions gameOptions = new();
    public MapData mapData = new();
    public HumanData humanData = new();


    public long GetNextId() {
        return nextId++;
    }

}
