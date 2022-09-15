using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerBehaviour : NPCBehaviour
{
    public ChairBehaviour assignedChair;
    private Meal servedMeal;
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
        while (!cafe.AssignSeat(this, out assignedChair))
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

        if (!cafe.TryMove(this, assignedChair.placedObject))
            Debug.LogError("Couldnt find a route to the assigned seat!");

        yield return new WaitUntil(() => HasReachedDestination()); //TODO: Add timeout, use waitwhile time elapsed < timeout time && !hasreacheddestination
        Debug.Log("Sitting to the table!");

        yield return PlaceOrder();
    }
    private IEnumerator PlaceOrder()
    {
        cafe.RequestMeal(this);
        yield return new WaitWhile(() => servedMeal == null);
        yield return Dine();
    }
    public void Serve(Meal meal)
    {
        servedMeal = meal;
    }
    private IEnumerator Dine()
    {
        Debug.Log("Dining "+ servedMeal.MealName);
        yield return new WaitForSeconds(5f);
        yield return Pay();
    }
    private IEnumerator Pay()
    {
        Debug.Log("Paying ");
        yield return Exit();
    }
    private IEnumerator Exit()
    {
        if(assignedChair != null)
        {
            assignedChair.Leave();
            cafe.TryMove(this, CafeSimulationManager.GetDoorTile());
        }
        yield return new WaitUntil(() => HasReachedDestination());
        Destroy(agent.gameObject, 2f);
    }

    public override void OnClick()
    {
        Debug.Log("Clicked on a customer!");
    }
}
