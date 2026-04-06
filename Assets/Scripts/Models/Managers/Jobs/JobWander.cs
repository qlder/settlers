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

    public override void Tick(int tickCount) {
        switch (state) {
            case State.Decide:
                Tile wanderToTarget = Map.Inst().GetRandomNearbyTile(entity.GetTile(), 10, 25);
                movementToTarget = new Movement(entity, wanderToTarget);
                movementToTarget.Start();
                state = State.Moving;
                this.actionName = "Wandering";
                break;
            case State.Moving:
                movementToTarget.MoveAlongPath();
                if (movementToTarget.state == Movement.State.Complete) {
                    Complete();
                }
                break;
        }
    }

    public override void Interrupt() {
        // Handle interruption logic if needed
    }

    public override void Complete() {
        // Handle completion logic if needed
        JobManager.Inst().RemoveJob(entity);
    }

}
