using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingOvenHUD : HUD
{
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI Name;
    [Space]
    [SerializeField] private Image ChefIcon;
    [SerializeField] private TextMeshProUGUI ChefName;
    [Space]
    [SerializeField] private Image MealIcon;
    [SerializeField] private TextMeshProUGUI MealName;
    [SerializeField] private TextMeshProUGUI MealTimer;

    private CookingOvenBehaviour cookingOven;
    public override void Open(BaseBehaviour behaviour)
    {
        cookingOven = (CookingOvenBehaviour)behaviour;

        Refresh();

        gameObject.SetActive(true);
    }
    public override void Refresh()
    {
        Icon.sprite = cookingOven.Icon;
        Name.text = cookingOven.Name;

        if (cookingOven.assignedChef != null)
        {
            ChefIcon.sprite = cookingOven.assignedChef.Icon;
            ChefName.text = cookingOven.assignedChef.Name;
        }
        else
        {
            ChefIcon.sprite = null;
            ChefName.text = "Select a chef to assign!";
        }

        if (cookingOven.meal != null)
        {
            MealIcon.sprite = cookingOven.meal.Icon;
            MealName.text = cookingOven.meal.MealName;
            MealTimer.text = "00:00";
        }
        else
        {
            MealIcon.sprite = null;
            MealName.text = "Select a meal to cook!";
            MealTimer.text = "";
        }
    }
    public override void Close()
    {
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if(cookingOven != null && cookingOven.IsCooking()) MealTimer.text = TimeSpan.FromSeconds(cookingOven.RemainingTime).ToString("mm':'ss");//(time.ToString("hh':'mm':'ss")); // 00:03:48
    }

    public void OpenAssignChefHUD()
    {
        ui.OpenHUD(cookingOven.assignedChef);
    }
    public void UpdateAssignedChef()
    {
        cookingOven.AssignChef(ui.GetSelectedChef());
        Refresh();
    }
    public void OpenCookMealHUD()
    {

    }
    public void CloseCookMealHUD()
    {

    }
}
