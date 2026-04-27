using System.Collections.Generic;
using UnityEngine;

public class Game {

    public static Game Inst;
    public GameData data = new();
    public GameSystem system = new();

    public void Tick(int ticks) {
        system.Tick(ticks);
    }


}
