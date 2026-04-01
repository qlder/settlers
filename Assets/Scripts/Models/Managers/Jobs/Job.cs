using System.Collections.Generic;
using UnityEngine;

public abstract class Job {

    public abstract void Tick(Entity entity, int tickCount);

}
