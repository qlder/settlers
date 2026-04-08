using System.Collections.Generic;
using UnityEngine;

public class Game {

    public static Game Inst;

    public GameOptions options { get; private set; }
    public Map map { get; private set; }
    public List<Entity> entities { get; private set; }

    public JobManager jobManager { get; private set; }
    public PathFinding pathFinding { get; private set; }
    public Controller controller { get; private set; }

    public Game(GameOptions options) {
        this.options = options;
        this.controller = new Controller();
        this.map = new Map(options);
        this.entities = new List<Entity>();
        this.jobManager = new JobManager();
        this.pathFinding = new PathFinding();
        this.pathFinding.Initialize();
    }

    //Powered by the kernel.
    public void Tick(int ticks) {

        controller.DealWithInput();
        // Debug.Log($"Tick: {tickCount} -- {frameCount} frames : {firstTick}");
        foreach (Entity entity in entities) {
            entity.Tick(ticks);
        }
    }

}
