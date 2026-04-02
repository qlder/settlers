using System.Collections.Generic;
using UnityEngine;

public class Entity
{

    public string name;

    public Tile currentTile;
    public float moveSpeed = 0.15f;

    public Job currentJob = null; //Change later...

    public void Tick(int tickCount)
    {
        if (currentJob == null)
        {
            if (tickCount % 30 == 0)
            { // assign a job every 30 ticks if no job
                currentJob = JobManager.Inst().GetJobForEntity(this);
            }
        }
        if (currentJob != null)
        {
            currentJob.Tick(tickCount);
        }
    }



}
