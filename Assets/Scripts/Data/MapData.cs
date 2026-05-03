using System.Collections.Generic;

public class MapData {

    public Dictionary<string, Tile> tiles = new();
    public Dictionary<string, Sector> sectors = new();

    public static MapData Inst() {
        return Game.Inst.data.mapData;
    }

}