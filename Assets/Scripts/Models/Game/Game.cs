using System.Collections.Generic;
using UnityEngine;

public class Game {

    public static Game Inst;

    public GameOptions options { get; private set; }

    public GameData data;
    // public Map map { get; private set; }
    // public List<Entity> entities { get; private set; }

    public PathFinding pathFinding { get; private set; }
    public TargetFinding targetFinding { get; private set; }
    public Controller controller { get; private set; }

    public Game(GameOptions options) {
        this.options = options;
        this.controller = new Controller();
        this.data = GameData.CreateNewGameData(options);
        this.pathFinding = new PathFinding();
        this.pathFinding.Initialize();
        this.targetFinding = new TargetFinding();
        this.targetFinding.Initialize();
    }

    //Powered by the kernel.
    public void Tick(int ticks) {

        controller.DealWithInput();
        // Debug.Log($"Tick: {tickCount} -- {frameCount} frames : {firstTick}");
        foreach (Entity entity in data.entities) {
            entity.Tick(ticks);
        }
    }

}
