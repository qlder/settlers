using System.Collections.Generic;

public enum PathRequestStatus {
    Pending,
    Running,
    Complete,
    Failed
}

public class PathRequest {
    public Tile Start { get; private set; }
    public Tile Target { get; private set; }

    public int StartX { get; private set; }
    public int StartZ { get; private set; }
    public int TargetX { get; private set; }
    public int TargetZ { get; private set; }

    public PathRequestStatus Status { get; internal set; }
    public Queue<Tile> Path { get; internal set; }

    public bool IsDone {
        get {
            return Status == PathRequestStatus.Complete || Status == PathRequestStatus.Failed;
        }
    }

    public bool Success {
        get {
            return Status == PathRequestStatus.Complete && Path != null && Path.Count > 0;
        }
    }

    public PathRequest(Tile start, Tile target) {
        Start = start;
        Target = target;

        StartX = start.X;
        StartZ = start.Z;
        TargetX = target.X;
        TargetZ = target.Z;

        Status = PathRequestStatus.Pending;
        Path = null;
    }
}