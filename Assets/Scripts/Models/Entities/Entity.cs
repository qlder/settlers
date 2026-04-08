using System.Collections.Generic;
using UnityEngine;

public class Entity {

    public string name;

    //Position and Movement
    public Pos currentPosition;
    public Pos headingPosition;

    public float moveSpeed = 0.025f;

    public Tile GetTile() {
        return currentPosition.GetTile();
    }

    public Job currentJob = null; //Change later...
    public void Tick(int ticks) {
        if (currentJob == null) {
            currentJob = JobManager.Inst().GetJobForEntity(this);
        }
        if (currentJob != null) {
            currentJob.Tick(ticks);
        }
    }



}
