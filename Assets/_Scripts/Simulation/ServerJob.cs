using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ServerJob
{
    protected static CafeSimulationManager cafe => CafeSimulationManager.instance;
    public abstract IEnumerator Execute(ServerBehaviour server);
}
