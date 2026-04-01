using System.Collections.Generic;
using UnityEngine;

public class JobWander : Job {

    //State Control
    enum State {
        Idle,
        PathFinding,
        Moving,
        Complete,
    }
    private State state;

    //Data Control
    private Tile wanderToTarget;

    public JobWander() {
        state = State.Idle;
    }

    public override void Tick(Entity entity, int tickCount) {
        switch (state) {
            case State.Idle:
                this.wanderToTarget = Map.Inst().GetRandomNearbyTile(entity.currentTile, 100, 150);
                state = State.PathFinding;
                break;
            case State.PathFinding:
                entity.path = PathFinding.Inst().GetPathForEntity(entity, wanderToTarget);
                // Debug.Log($"Entity {entity.name} is wandering to {wanderToTarget.X}, {wanderToTarget.Z}");
                state = State.Moving;
                break;
            case State.Moving:
                // Debug.Log($"Entity {entity.name} is moving along path to tickCount {tickCount}");
                entity.MoveAlongPath(tickCount);
                // state = State.Complete;
                if (entity.IsPathInvalid()) {
                    state = State.Complete;
                }
                break;
            case State.Complete:
                JobManager.Inst().RemoveJob(entity);
                // Job is complete, no further action
                break;
        }
    }

}
