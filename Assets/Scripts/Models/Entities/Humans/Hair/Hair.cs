using System.Collections.Generic;

public struct Hair : IModel {

    public long Id { get; set; }

    #region GetSet
    public static Hair? Get(long id) {
        //try to get or return null
        if (!LivingData.Inst().Hairs.TryGetValue(id, out var hair)) {
            return null;
        }
        return hair;
    }

    public void Save() {
        LivingData.Inst().Hairs[Id] = this;
    }
    #endregion


    public HairStyle style { get; set; }

    public enum HairStyle {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
        Type4 = 4,
        Type5 = 5,
        Type6 = 6,
    }



}
