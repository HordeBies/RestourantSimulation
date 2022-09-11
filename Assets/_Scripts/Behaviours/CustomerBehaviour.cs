using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerBehaviour : NPCBehaviour
{
    private void Start()
    {
        StartCoroutine(Enter());
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
                yield return Exit();
                yield break;
            }
        }

        yield return new WaitUntil(() => HasReachedDestination()); //TODO: Add timeout, use waitwhile time elapsed < timeout time && !hasreacheddestination
        agent.isStopped = true;
        Debug.Log("Sitting to the table!");

        yield return PlaceOrder();
    }
    private IEnumerator PlaceOrder()
    {
        float timeOut = 10f;
        while (!cafe.RequestMeal(this))
        {
            yield return new WaitForSeconds(1f);
            timeOut -= 1f;
            if (timeOut <= 0)
            {
                Debug.Log("Couldn't request a meal");
                yield return Exit();
                yield break;
            }
        }
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
        //Destroy(gameObject, 2f);
    }

    public override void OnClick()
    {
        Debug.Log("Clicked on a customer!");
    }
}
