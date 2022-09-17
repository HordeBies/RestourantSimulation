using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefBehaviour : NPCBehaviour
{
    CookingOvenBehaviour assignedCookingOven;
    public bool CanCook { private set; get; }
    private bool Lock = false;
    public IEnumerator AssignToCookingOven(CookingOvenBehaviour cookingOven)
    {
        yield return new WaitWhile(() => Lock);
        if (assignedCookingOven != null && assignedCookingOven != cookingOven) assignedCookingOven.AssignChef(null);
        if (assignedCookingOven == cookingOven) yield break;
        assignedCookingOven = cookingOven;
        CanCook = false;
        yield return MoveToOven();
    }
    public IEnumerator MoveToOven()
    {
        if (!cafe.TryMove(this, assignedCookingOven.placedObject, assignedCookingOven.placedObject.dir))
        {
            Debug.LogWarning("Chef can't reach to the destination");
        }
        yield return new WaitUntil(() => HasReachedDestination(true));
        CanCook = true;
        //Rotate into position
    }
    public IEnumerator TakeOutMeal(Meal meal)
    {
        Lock = true;
        ServingTableBehaviour emptyServingTable = default;
        yield return new WaitUntil(()=> cafe.GetServingTableForNewMeal(meal,out emptyServingTable));
        assignedCookingOven.ClearOven();
        CanCook = false;
        cafe.TryMove(this, emptyServingTable.placedObject);
        yield return new WaitUntil(()=> HasReachedDestination(true));
        emptyServingTable.PutMeal(meal);
        yield return MoveToOven();
        Lock = false;
    }
    public override void OnClick()
    {
        Debug.Log("Clicked on a chef!");
    }
}
