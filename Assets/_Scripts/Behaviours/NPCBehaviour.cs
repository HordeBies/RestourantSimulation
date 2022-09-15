using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPCBehaviour : BaseBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected AnimationManager anim;
    protected override void Awake()
    {
        base.Awake();
        if (agent != null)
        {
            //agent.transform.SetParent(null);
            //transform.SetParent(agent.transform);
            //agent.gameObject.name = transform.gameObject.name + "_NavMeshAgent";
        }
        else
        {
            Debug.LogError("NPC Behavior doesnt have a NavMeshAgent!");
        }
    }
    public bool Move(Vector3 to)
    {
        Debug.Log("Trying to Reach: " + to);
        agent.isStopped = false;
        return agent.SetDestination(to);
    }
    public bool HasReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.isStopped = true;
                    return true;
                }
            }
        }
        return false;
    }
}
