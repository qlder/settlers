using System.Collections.Generic;
using UnityEngine;

public class Entity {

    public string name;

    public Tile currentTile;
    public Queue<Tile> path = null;
    public float moveSpeed = 0.0001f;
    public float movingProgress = 0f;

    public Job currentJob = null; //Change later...

    public void Tick(int tickCount) {
        if (currentJob == null) {
            currentJob = JobManager.Inst().GetJobForEntity(this);
        }
        if (currentJob != null) {
            currentJob.Tick(this, tickCount);
        }
    }

    public void MoveAlongPath(int tickCount) {
        if (path == null || path.Count == 0) {
            return;
        }

        Tile nextTile = path.Peek();
        movingProgress += (moveSpeed * (float)tickCount);
        // Debug.Log($"Entity {name} moving to tile {nextTile.X}, {nextTile.Z} with movingProgress: {movingProgress}: {path.Count} tiles left");
        if (movingProgress < 1f) {
            return;
        }
        currentTile = nextTile;
        path.Dequeue();
        movingProgress = 0f;
        // Debug.Log($"Entity {name} moved to tile {currentTile.X}, {currentTile.Z} at tickCount {tickCount}");

    }
    public bool IsPathInvalid() {
        if (path == null || path.Count == 0) {
            return true;
        }
        return false;
    }


}
