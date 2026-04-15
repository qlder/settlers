using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class GameData {

    public static GameData Inst;

    public GameOptions options;
    public Map map;
    public List<Entity> entities;
    public JobManager jobManager;



    public static GameData CreateNewGameData(GameOptions options) {
        GameData data = new GameData();
        data.options = options;
        data.map = MapFactory.CreateMap(options);
        data.entities = new List<Entity>();
        data.jobManager = new JobManager();
        return data;
    }

    public void Save(string filePath) {
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        System.IO.File.WriteAllText(filePath, json);
    }

    public static void Load(string filePath) {
        if (!System.IO.File.Exists(filePath)) {
            return;
        }
        string json = System.IO.File.ReadAllText(filePath);
        JsonConvert.DeserializeObject<GameData>(json);
        //What next? We need to set the static instance of GameData to the loaded data, and also ensure that any necessary initialization is done after loading. Here's how we can do that:
        GameData loadedData = JsonConvert.DeserializeObject<GameData>(json);
        Game.Inst.data = loadedData; // Set the static instance to the loaded data 
    }


}
