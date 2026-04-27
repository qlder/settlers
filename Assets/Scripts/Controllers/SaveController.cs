using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveController {

    public static SaveController Inst() {
        return Controller.Inst.saveController;
    }

    public void DealWithInput() {

        if (Keyboard.current.pKey.wasPressedThisFrame) {
            Debug.Log("P key was pressed.");
            this.Save("savefile.json");
        }

        if (Keyboard.current.lKey.wasPressedThisFrame) {
            Debug.Log("L key was pressed.");
            this.Load("savefile.json");
        }

    }

    JsonSerializerSettings CreateSettings() {
        return new JsonSerializerSettings {
            Formatting = Formatting.Indented,
            Converters =
            {
                new Int2Converter(),
                new Float2Converter()
            }
        };
    }

    public void Save(string filePath) {
        string json = JsonConvert.SerializeObject(GameData.Inst(), CreateSettings());
        System.IO.File.WriteAllText(filePath, json);
    }

    public void Load(string filePath) {
        if (!System.IO.File.Exists(filePath))
            return;

        string json = System.IO.File.ReadAllText(filePath);
        GameData loadedData = JsonConvert.DeserializeObject<GameData>(json, CreateSettings());
        Game.Inst.data = loadedData;
    }

}