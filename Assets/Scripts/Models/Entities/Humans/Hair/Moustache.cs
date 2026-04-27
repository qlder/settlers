using System.Collections.Generic;

public struct Moustache : IModel {

    public long Id { get; set; }

    #region GetSet
    public static Moustache? Get(long id) {
        //try to get or return null
        if (!HumanData.Inst().Moustaches.TryGetValue(id, out var moustache)) {
            return null;
        }
        return moustache;
    }

    public void Save() {
        HumanData.Inst().Moustaches[Id] = this;
    }
    #endregion


    public MoustacheStyle style { get; set; }

    public enum MoustacheStyle {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
        Type4 = 4,
        Type5 = 5,
    }



}
