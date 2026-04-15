using System.Collections.Generic;

public enum TargetRequestStatus {
    Pending,
    Running,
    Complete,
    Failed
}

public class TargetRequest {

    public int StartX { get; private set; }
    public int StartZ { get; private set; }

    public TargetRequestStatus Status { get; internal set; }
    public object Target { get; internal set; }

    // public bool IsDone {
    //     get {
    //         return Status == TargetRequestStatus.Complete || Status == TargetRequestStatus.Failed;
    //     }
    // }

    // public bool Success {
    //     get {
    //         return Status == TargetRequestStatus.Complete && Path != null && Path.Count > 0;
    //     }
    // }

    // public TargetRequest(Tile start, TargetCondition condition) {
    //     Start = start;
    //     Target = condition;

    //     StartX = start.X;
    //     StartZ = start.Z;
    //     TargetX = target.X;
    //     TargetZ = target.Z;

    //     Status = PathRequestStatus.Pending;
    //     Path = null;
    // }
}