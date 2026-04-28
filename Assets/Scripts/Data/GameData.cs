using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class GameData {

    public static GameData Inst() {
        return Game.Inst.data;
    }

    public long nextId = 1;
    public uint rngState = 0; // Will be set by Rng class.

    public GameOptions gameOptions = new();
    public MapData mapData = new();
    public LivingData livingData = new();
    public JobData jobData = new();

    public long GetNextId() {
        return nextId++;
    }

}
