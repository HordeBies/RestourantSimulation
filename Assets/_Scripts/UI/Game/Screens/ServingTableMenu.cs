using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bies.Game.UI
{
    public class ServingTableMenu : ScreenBase
    {
        public event Action OnBackgroundButtonClick;
        public event Action<ServingTableMenu> OnClearButtonClick;

        private const string k_BackgroundButton = "background-button";
        private const string k_ClearButton = "clear-button";
        private const string k_MealIcon = "meal-icon";
        private const string k_MealName = "meal-name";
        private const string k_MealPrice = "meal-price";
        private const string k_MealServings = "meal-servings";


        VisualElement BackgroundButton;
        VisualElement ClearButton;

        VisualElement MealIcon;
        Label MealName;
        Label MealPrice;
        Label MealServings;

        ServingTableBehaviour reference;
        public ServingTableBehaviour GetData() => reference;

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            BackgroundButton = Root.Q(k_BackgroundButton);
            ClearButton = Root.Q(k_ClearButton);

            
            MealIcon = Root.Q(k_MealIcon);
            MealName = Root.Q<Label>(k_MealName);
            MealPrice = Root.Q<Label>(k_MealPrice);
            MealServings = Root.Q<Label>(k_MealServings);
        }

        protected override void RegisterButtonCallbacks()
        {
            BackgroundButton.RegisterCallback<ClickEvent>(BackgroundButtonClicked);
            ClearButton.RegisterCallback<ClickEvent>(ClearButtonClicked);
        }
        private void BackgroundButtonClicked(ClickEvent evt)
        {
            OnBackgroundButtonClick?.Invoke();
        }
        private void ClearButtonClicked(ClickEvent evt)
        {
            OnClearButtonClick?.Invoke(this);
        }
        public void Populate(ServingTableBehaviour data)
        {
            reference = data;
            Refresh();
        }
        private void Refresh()
        {
            ShowVisualElement(ClearButton, reference.meal != null);

            if(reference.meal != null)
            {
                MealIcon.style.backgroundImage = new(reference.meal.Icon);
                MealName.text = reference.meal.MealName;
                MealServings.text = reference.remainingServings.ToString();
                MealPrice.text = "99$"; //TODO: Add Meal Price to the Meal SO
            }
            else
            {
                MealIcon.style.backgroundImage = new();
                MealName.text = "Empty";
                MealServings.text = "0";
                MealPrice.text = "";
            }

        }
        private void FixedUpdate()
        {
            if (!IsVisible()) return;
            if(reference != null) Refresh();
        }
    }
}
