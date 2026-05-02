using System.Collections.Generic;
using Unity.Mathematics;

public struct Position : IModel {

    public int Id { get; set; }

    #region GetSet
    public static Position? Get(int id) {
        //try to get or return null
        if (!LivingData.Inst().Positions.TryGetValue(id, out var position)) {
            return null;
        }
        return position;
    }

    public void Save() {
        LivingData.Inst().Positions[Id] = this;
    }
    #endregion

    public float2 coords { get; set; }
    public Facing facing { get; set; }

    public Tile GetTile() {
        string key = $"{(int)coords.x}_{(int)coords.y}";
        return MapData.Inst().tiles[key];
    }



}
