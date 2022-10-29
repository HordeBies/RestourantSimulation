using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bies.Game.UI
{
    public class CookingOvenMenu : ScreenBase
    {
        public event Action OnBackgroundButtonClick;
        public event Action<CookingOvenMenu> OnChefButtonClick;
        public event Action<CookingOvenMenu> OnMealButtonClick;

        private const string k_BackgroundButton = "background-button";
        private const string k_OvenIcon = "oven-icon";
        private const string k_OvenLabel = "oven-label";
        private const string k_ChefButton = "chef-button";
        private const string k_ChefIcon = "chef-icon";
        private const string k_ChefLabel = "chef-label";
        private const string k_MealButton = "meal-button";
        private const string k_MealIcon = "meal-icon";
        private const string k_MealLabel = "meal-label";
        private const string k_MealTimer = "meal-timer";


        VisualElement BackgroundButton;

        VisualElement OvenIcon;
        Label OvenLabel;

        VisualElement ChefButton;
        VisualElement ChefIcon;
        Label ChefLabel;

        VisualElement MealButton;
        VisualElement MealIcon;
        Label MealLabel;
        Label MealTimer;

        CookingOvenBehaviour reference;
        public CookingOvenBehaviour GetData() => reference;
        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            BackgroundButton = Root.Q(k_BackgroundButton);

            OvenIcon = Root.Q(k_OvenIcon);
            OvenLabel = Root.Q<Label>(k_OvenLabel);

            ChefButton = Root.Q(k_ChefButton);
            ChefIcon = Root.Q(k_ChefIcon);
            ChefLabel = Root.Q<Label>(k_ChefLabel);

            MealButton = Root.Q(k_MealButton);
            MealIcon = Root.Q(k_MealIcon);
            MealLabel = Root.Q<Label>(k_MealLabel);
            MealTimer = Root.Q<Label>(k_MealTimer);
        }

        protected override void RegisterButtonCallbacks()
        {
            BackgroundButton.RegisterCallback<ClickEvent>(BackgroundButtonClicked);
            ChefButton.RegisterCallback<ClickEvent>(ChefButtonClicked);
            MealButton.RegisterCallback<ClickEvent>(MealButtonClicked);
        }
        private void BackgroundButtonClicked(ClickEvent evt)
        {
            OnBackgroundButtonClick?.Invoke();
        }
        private void ChefButtonClicked(ClickEvent evt)
        {
            OnChefButtonClick?.Invoke(this);
        }
        private void MealButtonClicked(ClickEvent evt)
        {
            OnMealButtonClick?.Invoke(this);
        }

        public void Populate(CookingOvenBehaviour data)
        {
            reference = data;
            Refresh();
        }
        private void Refresh()
        {
            OvenIcon.style.backgroundImage = new(reference.Icon);
            OvenLabel.text = reference.Name;
            if (reference.assignedChef != null)
            {
                ChefIcon.style.backgroundImage = new(reference.assignedChef.Icon);
                ChefLabel.text = reference.assignedChef.Name;
            }
            else
            {
                //TODO: Bind Defaults
                ChefIcon.style.backgroundImage = new();
                ChefLabel.text = "Select a chef to assign";
            }
            if (reference.meal != null)
            {
                MealIcon.style.backgroundImage = new(reference.meal.Icon);
                MealLabel.text = reference.meal.MealName;
                MealTimer.text = reference.RemainingTimeFormatted;
            }
            else
            {
                //TODO: Bind Defaults
                MealIcon.style.backgroundImage = new();
                MealLabel.text = "Select a meal to prepare";
                MealTimer.text = "";
            }
        }
        private void FixedUpdate()
        {
            if (!IsVisible()) return;
            if(reference != null) Refresh();
        }
    }
}
