using System.Collections.Generic;
using UnityEngine;

public class JobManager {

    public static JobManager Inst() {
        return Game.Inst.data.jobManager;
    }

    public Job GetJobForEntity(Entity entity) {
        return new JobWander(entity);
    }

    public void InterruptAllJobs() {
        foreach (Entity entity in Game.Inst.data.entities) {
            if (entity.currentJob != null) {
                entity.currentJob.Interrupt();
                entity.currentJob = null;
            }
        }
        // Implement logic to interrupt all jobs, e.g., by keeping track of active jobs and calling Interrupt on each
    }

    public void RemoveJob(Entity entity) {
        entity.currentJob = null;
    }

}
