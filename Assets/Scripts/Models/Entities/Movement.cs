using System.Collections.Generic;
using UnityEngine;

public class Movement
{

    public Entity entity;
    public Tile targetTile;
    public Queue<Tile> path = null;

    public float movingProgress = 0f;

    public enum State
    {
        Pending,
        PathFinding,
        Moving,
        Complete,
        Invalid,
    }

    public State state;
    public Movement(Entity entity, Tile targetTile)
    {
        this.entity = entity;
        this.targetTile = targetTile;
        this.state = State.Pending;
    }

    public void Start()
    {
        this.state = State.PathFinding;
        this.path = PathFinding.Inst().GetPathForEntity(entity, targetTile);
        this.state = State.Moving;
    }


    public void MoveAlongPath()
    {
        if (this.state != State.Moving)
        {
            return;
        }
        if (path == null)
        {
            this.state = State.Invalid;
            return;
        }

        Tile nextTile = path.Peek();
        movingProgress += this.entity.moveSpeed;
        if (movingProgress < 1f)
        {
            return;
        }
        this.entity.currentTile = nextTile;
        path.Dequeue();
        movingProgress = movingProgress - 1f;
        if (path.Count == 0)
        {
            this.state = State.Complete;
        }
        // TODO: Deal with multiple tile movement in one tick if moveSpeed is high enough
        // Debug.Log($"Entity {name} moved to tile {currentTile.X}, {currentTile.Z} at tickCount {tickCount}");

    }



}
