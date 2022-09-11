using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UISlot : MonoBehaviour
{
    public void Populate(GridObject gridObject, Action<UISlot> onClickAction)
    {
        this.gridObject = gridObject;
        if(gridObject != null)
        {
            Icon.sprite = gridObject.icon;
            Name.text = gridObject.nameString;
        }

        OnClickAction = onClickAction;
    }
    public void Populate(Meal meal, Action<UISlot> onClickAction)
    {
        this.meal = meal;
        if(meal != null)
        {
            Icon.sprite = meal.Icon;
            Name.text = meal.MealName;
            Description.text = $"Servings: {meal.Servings}\tPrepTime: {TimeSpan.FromSeconds(meal.PrepTime).ToString("mm':'ss")}";
        }
        
        OnClickAction = onClickAction;
    }
    public void Populate(ChefBehaviour chef, Action<UISlot> onClickAction)
    {
        this.chef = chef;
        if (chef != null)
        {
            Icon.sprite = chef.Icon;
            Name.text = chef.Name;
        }

        OnClickAction = onClickAction;
    }

    public Button Button;
    public GameObject SelectionHighlight;
    public Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;

    public GridObject gridObject;
    public Meal meal;
    public ChefBehaviour chef;

    private Action<UISlot> OnClickAction;
    public void OnClick()
    {
        OnClickAction?.Invoke(this);
    }
    public void Refresh(ChefBehaviour selectedChef)
    {
        SelectionHighlight.SetActive(chef == selectedChef);
    }
    
}
