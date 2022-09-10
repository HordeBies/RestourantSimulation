using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerBehaviour : BaseBehaviour
{
    
    private void Start()
    {
        agent.speed = MovementSpeed;
        StartCoroutine(Enter());
    }
    public bool Move(Vector3 to)
    {
        Debug.Log("Trying to Reach: " + to);
        return agent.SetDestination(to);
    }
    private IEnumerator Enter()
    {
        yield return Sit();
    }
    private IEnumerator Sit()
    {
        float timeOut = 10f; 
        while (!cafe.AssignSeat(this))
        {
            yield return new WaitForSeconds(1f);
            timeOut -= 1f;
            if(timeOut <= 0)
            {
                Debug.Log("Couldn't find an empty seat");
                yield break;
            }
        }

        yield return new WaitUntil(() => HasReachedDestination()); //TODO: Add timeout, use waitwhile time elapsed < timeout time && !hasreacheddestination

        Debug.Log("Sitting to the table!");

        yield return PlaceOrder();
    }
    private IEnumerator PlaceOrder()
    {
        yield return Dine();
    }
    private IEnumerator Dine()
    {
        yield return Pay();
    }
    private IEnumerator Pay()
    {
        yield return Exit();
    }
    private IEnumerator Exit()
    {
        yield return null;
    }

    public override void OnClick()
    {
        Debug.Log("Clicked on a customer!");
    }
}
