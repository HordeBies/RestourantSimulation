using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    void Start()
    {
        // Don’t update position automatically
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.autoBraking = false;
    }

    void Update()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - agent.transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(agent.transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(agent.transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.isActiveAndEnabled && agent.remainingDistance > agent.radius;

        // Pull character towards agent
        if (worldDeltaPosition.magnitude > agent.radius)
            transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;
        //// Pull agent towards character
        //if (worldDeltaPosition.magnitude > agent.radius)
        //    agent.nextPosition = agent.transform.position + 0.9f * worldDeltaPosition;

        // Update animation parameters
        anim.SetBool("move", shouldMove);
        anim.SetFloat("velx", velocity.x);
        anim.SetFloat("vely", velocity.y);
    }
    public void Sit()
    {
        anim.SetTrigger("sit");
    }
    void OnAnimatorMove()
    {
        // Update position to agent position
        Vector3 position = anim.rootPosition;
        position.y = agent.nextPosition.y;
        agent.transform.position = position;

        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            agent.transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
    }
}
