using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanData {

    public static HumanData Inst() {
        return Game.Inst.data.humanData;
    }

    public Dictionary<long, Human> Humans = new();

    // parentId -> childIds
    public OneToMany Fathers = new();
    public OneToMany Mothers = new();




}