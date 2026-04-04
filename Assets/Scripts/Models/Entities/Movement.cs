using System.Collections.Generic;
using UnityEngine;

public class Movement {

    public Entity entity;
    public Tile targetTile;
    public Queue<Tile> path = null;

    public enum State {
        Pending,
        PathFinding,
        Moving,
        Complete,
        Invalid,
    }

    public State state;
    public Movement(Entity entity, Tile targetTile) {
        this.entity = entity;
        this.targetTile = targetTile;
        this.state = State.Pending;
    }

    public void Start() {
        this.state = State.PathFinding;
        this.path = PathFinding.Inst().GetPathForEntity(entity, targetTile);
        this.state = State.Moving;
    }


    public void MoveAlongPath() {
        if (this.state != State.Moving) {
            return;
        }
        if (path == null) {
            this.state = State.Invalid;
            return;
        }
        if (path.Count == 0) {
            this.state = State.Complete;
            return;
        }
        Tile nextTile = path.Peek();
        entity.headingPosition = nextTile.GetPosition();

        float distance = Vector2.Distance(entity.currentPosition.GetVector2(), entity.headingPosition.GetVector2());
        float step = entity.moveSpeed;


        if (step < distance) {
            entity.currentPosition = new Pos(Vector2.MoveTowards(
                entity.currentPosition.GetVector2(),
                entity.headingPosition.GetVector2(),
                step
            ));
        } else {
            entity.currentPosition = entity.headingPosition;
            path.Dequeue();
            if (path.Count == 0) {
                this.state = State.Complete;
            }
        }

    }



}
