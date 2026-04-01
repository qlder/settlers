using System.Collections.Generic;
using UnityEngine;

public class JobManager {

    public static JobManager Inst() {
        return Game.Inst.jobManager;
    }

    public Job GetJobForEntity(Entity entity) {
        return new JobWander();
    }

    public void RemoveJob(Entity entity) {
        entity.currentJob = null;
    }

}
