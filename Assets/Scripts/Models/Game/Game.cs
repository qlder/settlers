using System.Collections.Generic;
using UnityEngine;

public class Game {

    public static Game Inst;

    public GameOptions options { get; private set; }
    public Map map { get; private set; }
    public List<Human> humans { get; private set; }
    public List<Animal> animals { get; private set; }

    public JobManager jobManager { get; private set; }
    public PathFinding pathFinding { get; private set; }
    public PathFindingThreader pathFindingThreader { get; private set; }

    public Game(GameOptions options) {
        this.options = options;
        this.map = new Map(options);
        this.humans = new List<Human>();
        this.animals = new List<Animal>();
        this.jobManager = new JobManager();
        this.pathFinding = new PathFinding();
        this.pathFindingThreader = new PathFindingThreader();
        this.pathFindingThreader.Start();
    }

    //Powered by the kernel.
    public void Tick(int tickCount, int frameCount, bool firstTick) {
        // Debug.Log($"Tick: {tickCount} -- {frameCount} frames : {firstTick}");
        foreach (Human human in humans) {
            human.Tick(tickCount);
        }
    }

}
