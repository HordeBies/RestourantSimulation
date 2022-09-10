using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseBehaviour : MonoBehaviour
{
    protected CafeSimulationManager cafe;
    public PlacedObject placedObject;

    [Header("NPC")]
    [SerializeField]protected NavMeshAgent agent;
    [SerializeField] protected float MovementSpeed = 10f;
    private void Awake()
    {
        placedObject = GetComponent<PlacedObject>();
        cafe = CafeSimulationManager.instance;

        if(agent!= null)
        {
            agent.transform.SetParent(null);
            transform.SetParent(agent.transform);
        }
    }
    protected bool HasReachedDestination()
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
    public abstract void OnClick();
    public GridObject GetData()
    {
        return placedObject.placedObjectTypeSO;
    }
}
