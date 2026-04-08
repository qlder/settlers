using System.Collections.Generic;
using UnityEngine;

public abstract class Job {

    public string actionName = "TEST JOB";
    public abstract void Tick(int ticks);

    public abstract void Interrupt();
    public abstract void Complete();

}
