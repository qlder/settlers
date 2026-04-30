using System.Collections.Generic;
using UnityEngine;

public class Game {

    public static Game Inst;
    public GameData data = new();
    public GameSystem system = new();

    public void Tick(int ticks) {
        system.Tick(ticks);
        Debug.Log($"LivingData.Inst().Entities.Count: {LivingData.Inst().Entities.Count}");
        Debug.Log($"LivingData.Inst().Fathers.ownerMap.Count: {LivingData.Inst().Fathers.GetOwnerMap.Count}");
    }


}
