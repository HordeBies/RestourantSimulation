using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingTableBehaviour : BaseBehaviour
{
    public Meal meal { private set; get; }
    public int remainingServings;
    [SerializeField] Transform mealPos;
    private GameObject mealPrefab;
    public void PutMeal(Meal meal)
    {
        if(this.meal == null)
        {
            this.meal = meal;
            remainingServings = meal.Servings;
            mealPrefab = Instantiate(meal.prefab, mealPos.position, Quaternion.Euler(0, GetData().GetRotationAngle(placedObject.dir), 0), mealPos).gameObject;
        }else if(this.meal == meal)
        {
            remainingServings += meal.Servings;
        }
    }
    public bool Empty()
    {
        return meal == null;
    }
    public Meal Serve()
    {
        var servedMeal = meal;
        remainingServings--;
        if(remainingServings == 0)
        {
            ClearTable();
        }
        return servedMeal;
    }
    public void ClearTable()
    {
        meal = null;
        remainingServings = 0;
        Destroy(mealPrefab);
    }
    public override void OnClick()
    {
        cafe.ui.ShowMenu(this);
        Debug.Log("Clicked on a serving table");
    }
}
