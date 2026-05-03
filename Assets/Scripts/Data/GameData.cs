using System.Collections.Generic;
using UnityEngine;
using SQLite;

[System.Serializable]
public class GameData {

    public static GameData Inst() {
        return Game.Inst.data;
    }

    public GameData() {
        Debug.LogWarning("New GameData created. This should only happen when starting a new game.");
        var db = new SQLiteConnection(":memory:");
        Debug.Log("Creating tables...");
        db.CreateTable<GameOptions>();
        db.CreateTable<Tile>();
        Debug.Log("DONE");
        Debug.Log(db.Table<Tile>().Count());

        var result = db.Query<TableInfo>("PRAGMA table_info(Tile);");
        foreach (var column in result) {
            Debug.Log($"{column.name} ({column.type})");
        }
        string schema = db.ExecuteScalar<string>(
            "SELECT sql FROM sqlite_master WHERE type='table' AND name='Tile';"
        );
        Debug.Log(schema);
    }


    public uint rngState = 0; // Will be set by Rng class.

    public GameOptions gameOptions = new();
    public MapData mapData = new();
    public LivingData livingData = new();
    public JobData jobData = new();
    public CameraData cameraData = new();



}

public class TableInfo {
    public int cid { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public int notnull { get; set; }
    public string dflt_value { get; set; }
    public int pk { get; set; }
}
