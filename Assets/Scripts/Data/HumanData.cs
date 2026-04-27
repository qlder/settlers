using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanData {

    public static HumanData Inst() {
        return Game.Inst.data.humanData;
    }

    //Based on ID
    public Dictionary<long, Human> Humans = new();
    public Dictionary<long, HumanDNA> HumanDNA = new();
    public Dictionary<long, Hair> Hairs = new();
    public Dictionary<long, Moustache> Moustaches = new();
    public Dictionary<long, Beard> Beards = new();

    // parentId -> childIds
    public OneToMany Fathers = new();
    public OneToMany Mothers = new();




}