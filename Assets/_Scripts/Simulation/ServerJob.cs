using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ServerJob
{
    protected static CafeSimulationManager cafe => CafeSimulationManager.instance;
    protected ServerBehaviour server;
    public virtual IEnumerator Execute(ServerBehaviour server)
    {
        this.server = server;
        yield break;
    }
    public void Interrupt()
    {
        //TODO: Add interrupt 
    }
}
