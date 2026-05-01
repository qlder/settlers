using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LivingData {

    public static LivingData Inst() {
        return Game.Inst.data.livingData;
    }

    //Based on ID

    public Dictionary<long, Entity> Entities = new();

    //All Entities
    public Dictionary<long, Genetics> Genetics = new();

    //On Map Entities
    public Dictionary<long, Position> Positions = new();
    public Dictionary<long, Path> Paths = new();

    //Human Only
    public Dictionary<long, Hair> Hairs = new();
    public Dictionary<long, Moustache> Moustaches = new();
    public Dictionary<long, Beard> Beards = new();


    // parentId -> childIds
    public OneToMany Fathers = new();
    public OneToMany Mothers = new();




}