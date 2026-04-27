using System.Collections.Generic;

public struct Beard : IModel {

    public long Id { get; set; }

    #region GetSet
    public static Beard? Get(long id) {
        //try to get or return null
        if (!HumanData.Inst().Beards.TryGetValue(id, out var beard)) {
            return null;
        }
        return beard;
    }

    public void Save() {
        HumanData.Inst().Beards[Id] = this;
    }
    #endregion


    public BeardStyle style { get; set; }

    public enum BeardStyle {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
        Type4 = 4,
        Type5 = 5,
    }



}
