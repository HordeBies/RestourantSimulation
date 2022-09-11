using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefBehaviour : NPCBehaviour
{
    CookingOvenBehaviour assignedCookingOven;
    public void AssignToCookingOven(CookingOvenBehaviour cookingOven)
    {
        if (assignedCookingOven != null && assignedCookingOven != cookingOven) assignedCookingOven.AssignChef(null);
        if (assignedCookingOven == cookingOven) return;
        assignedCookingOven = cookingOven;
        if(!cafe.TryMove(this, assignedCookingOven.placedObject, assignedCookingOven.placedObject.dir))
        {
            Debug.LogWarning("Chef can't reach to the destination");
        }
        StartCoroutine(CorrectRotation());
    }
    public IEnumerator CorrectRotation()
    {
        yield return new WaitUntil(() => HasReachedDestination());
        agent.isStopped = true;
        //Rotate into position
    }
    public override void OnClick()
    {
        Debug.Log("Clicked on a chef!");
    }
}
