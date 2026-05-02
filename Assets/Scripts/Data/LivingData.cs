using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LivingData {

    public static LivingData Inst() {
        return Game.Inst.data.livingData;
    }

    public int nextId = 1;
    public int GetNextId() {
        return nextId++;
    }

    //Based on ID

    public Dictionary<int, Entity> Entities = new();

    //All Entities
    public Dictionary<int, Genetics> Genetics = new();

    //On Map Entities
    public Dictionary<int, Position> Positions = new();
    public Dictionary<int, Path> Paths = new();

    //Human Only
    public Dictionary<int, Hair> Hairs = new();
    public Dictionary<int, Moustache> Moustaches = new();
    public Dictionary<int, Beard> Beards = new();

    public Dictionary<int, int> JobIds = new();


    // parentId -> childIds
    public OneToMany Fathers = new();
    public OneToMany Mothers = new();




}