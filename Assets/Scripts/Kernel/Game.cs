using System.Collections.Generic;
using UnityEngine;

public class Game {

    public static Game Inst;
    public GameData data = new();
    public GameSystem system = new();

    System.Random random = new();

    public static System.Random Random() {
        return Inst.random;
    }

    public void Tick(int ticks) {
        system.Tick(ticks);
    }


}
