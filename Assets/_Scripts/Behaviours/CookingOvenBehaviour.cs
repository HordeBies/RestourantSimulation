using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingOvenBehaviour : BaseBehaviour
{
    public ChefBehaviour assignedChef { private set; get; }
    public Meal meal { private set; get; }
    public float RemainingTime { private set; get; }
    [SerializeField] private GameObject cookingPot;
    [SerializeField] private Transform mealPos;
    private GameObject mealPrefab;

    public bool IsAvailable()
    {
        return assignedChef != null && meal == null; 
    }
    public bool IsCooking()
    {
        return meal != null;
    }
    public void AssignChef(ChefBehaviour chef)
    {
        assignedChef = chef;
        if(chef != null) chef.StartCoroutine(chef.AssignToCookingOven(this));
    }
    public void Cook(Meal mealToCook)
    {
        StartCoroutine(StartCooking(mealToCook));
        Debug.Log("Starting to cook " + mealToCook.MealName);
    }
    private bool CanCook()
    {
        return assignedChef != null && assignedChef.CanCook;
    }
    private IEnumerator StartCooking(Meal mealToCook)
    {
        meal = mealToCook;
        RemainingTime = meal.PrepTime;
        cookingPot?.SetActive(true);
        while(RemainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            if (CanCook())
            {
                RemainingTime -= 1f;
            }
        }
        cookingPot.SetActive(false);
        mealPrefab = Instantiate(meal.prefab, mealPos.position, Quaternion.Euler(0, GetData().GetRotationAngle(placedObject.dir), 0),mealPos).gameObject;
        Debug.Log("Finished cooking " + mealToCook.MealName);
        
        yield return assignedChef.TakeOutMeal(meal);
    }
    public void ClearOven()
    {
        Destroy(mealPrefab);
        meal = null;
    }
    public override void OnClick()
    {
        Debug.Log("Clicked on a cooking oven!");
        cafe.ui.OpenHUD(this);
    }
}
