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
        FixSit();
        Debug.Log("Sitting to the table!");
        yield return PlaceOrder();
    }
    private void FixSit()
    {
        //agent.baseOffset += 100f;
        model.parent = transform;
        model.position = assignedChair.customerPlace.position;
        model.rotation = Quaternion.Euler(0, GetData().GetRotationAngle(assignedChair.TableDir), 0);
        controller.SitDown();
        agent.gameObject.SetActive(false);
    }
    private void FixStandUp()
    {
        //agent.baseOffset -= 100f;
        agent.gameObject.SetActive(true);
        model.parent = agent.transform;
        model.localPosition = Vector3.zero;
        model.localRotation = Quaternion.identity;
        controller.StandUp();
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
        FixStandUp();
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
        Destroy(gameObject, 2f);
        cafe.CustomerLeaved(this);
    }
    public void ForceLeave()
    {
        FixStandUp();
        StartCoroutine(Exit());
    }
    public override void OnClick()
    {
        Debug.Log("Clicked on a customer!");
    }
}
