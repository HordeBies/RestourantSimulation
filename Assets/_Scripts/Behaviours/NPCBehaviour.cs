using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public abstract class NPCBehaviour : BaseBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected ThirdPersonCharacter controller;
    [SerializeField] protected Transform model;
    [Space]
    private float MovementSpeed = 17.5f;
    private float AngularSpeed = 120f;
    private float Acceleration = 40f;


    protected override void Awake()
    {
        base.Awake();
        if (agent != null)
        {
            //agent.transform.SetParent(null);
            //transform.SetParent(agent.transform);
            //agent.gameObject.name = transform.gameObject.name + "_NavMeshAgent";
            agent.updateRotation = false;
            agent.speed = MovementSpeed;
            agent.angularSpeed = AngularSpeed;
            agent.acceleration = Acceleration;
        }
        else
        {
            Debug.LogError("NPC Behavior doesnt have a NavMeshAgent!");
        }
    }
    protected virtual void Update()
    {
        if (controller == null) return;
        if (!HasReachedDestination())
            controller.Move(agent.desiredVelocity, false, false);
        else
            controller.Move(Vector3.zero, false, false);
    }
    public bool Move(Vector3 to)
    {
        Debug.Log("Trying to Reach: " + to);
        agent.isStopped = false;
        return agent.SetDestination(to);
    }
    protected void PostNavRotationSnap(Vector3 to)
    {
        var current = model.rotation.eulerAngles;
        model.rotation = Quaternion.Euler(new (current.x,Mathf.Round(current.y / 90) * 90,current.z));
    }
    public bool HasReachedDestination(bool postFix = false)
    {
        if (!agent.isActiveAndEnabled)
        {
            return true;
        }
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
