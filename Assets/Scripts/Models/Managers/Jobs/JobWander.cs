using System.Collections.Generic;
using UnityEngine;

public class JobWander : Job {

    private Entity entity;
    //State Control
    enum State {
        Decide,
        Moving,
    }
    private State state;

    //Data Control
    private Movement movementToTarget;

    public JobWander(Entity entity) {
        this.entity = entity;
        state = State.Decide;
        this.actionName = "Thinking";
    }

    public override void Tick(int ticks) {
        switch (state) {
            case State.Decide:
                Tile wanderToTarget = Map.Inst().GetRandomNearbyTile(entity.GetTile(), 200, 250);
                if (wanderToTarget == null) {
                    // Debug.LogError("No valid wander target found.");
                    return;
                }
                movementToTarget = new Movement(entity, wanderToTarget);
                movementToTarget.Start();
                state = State.Moving;
                this.actionName = "Wandering";
                break;
            case State.Moving:
                movementToTarget.MoveAlongPath(ticks);
                if (movementToTarget.state == Movement.State.Complete) {
                    Complete();
                }
                break;
        }
    }

    public override void Interrupt() {
        Debug.Log("Wander job interrupted for entity: " + entity.name);
        Complete();
    }

    public override void Complete() {
        // Handle completion logic if needed
        JobManager.Inst().RemoveJob(entity);
    }

}
