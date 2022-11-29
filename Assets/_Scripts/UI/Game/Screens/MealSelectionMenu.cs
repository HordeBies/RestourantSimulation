using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bies.Game.UI
{
    public class MealSelectionMenu : ScreenBase
    {
        public event Action<CookingOvenBehaviour,Meal> OnPrepareButtonClick;
        public event Action OnCancelButtonClick;


        //private const string k_BackgroundButton = "background-button";
        private const string k_ContentParent = "content-parent";
        private const string k_PrepareButton = "assign-button";
        private const string k_CancelButton = "cancel-button";

        private const string k_ContentButton = "content-button";
        private const string k_ContentHighlight = "content-highlight";
        private const string k_ContentMealName = "meal-name";
        private const string k_ContentMealIcon = "meal-icon";
        private const string k_ContentMealServings = "meal-servings";
        private const string k_ContentMealPortionPrice = "meal-portion-price";
        private const string k_ContentMealPrepPrice = "meal-prep-price";
        private const string k_ContentMealPrepTime = "meal-prep-time";


        //VisualElement BackgroundButton; //Can add "click anywhere else to cancel functionality" with this
        VisualElement ContentParent;
        VisualElement PrepareButton;
        VisualElement CancelButton;

        CookingOvenMenu Parent;
        Content<GameData.MealData> SelectedContent;
        List<Content<GameData.MealData>> Contents;
        [SerializeField] VisualTreeAsset TabContent;

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            //BackgroundButton = Root.Q(k_BackgroundButton);
            ContentParent = Root.Q(k_ContentParent);
            PrepareButton = Root.Q(k_PrepareButton);
            CancelButton = Root.Q(k_CancelButton);

        }
        protected override void RegisterButtonCallbacks()
        {
            PrepareButton.RegisterCallback<ClickEvent>(PrepareButtonClicked);
            CancelButton.RegisterCallback<ClickEvent>(CancelButtonClicked);
        }
        private void PrepareButtonClicked(ClickEvent evt)
        {
            OnPrepareButtonClick?.Invoke(Parent.GetData(),SelectedContent.Data.meal);
        }
        private void CancelButtonClicked(ClickEvent evt)
        {
            OnCancelButtonClick?.Invoke();
        }
        private void ClearContents()
        {
            Parent = null;
            Contents = new();
            SelectedContent = null;
            ContentParent.Clear();
        }
        private void SelectContent(Content<GameData.MealData> content)
        {
            if (SelectedContent == content) return;
            if (content.Locked(content.ContentButton, content.Data)) return;
            SelectedContent = content;
            UpdateContentHighlights();
        }
        private void UpdateContentHighlights()
        {
            foreach (var content in Contents)
            {
                content.ContentHighlight.style.visibility = (content == SelectedContent) ? Visibility.Visible : Visibility.Hidden;
            }
        }
        public void Populate(List<GameData.MealData> data, CookingOvenMenu parent)
        {
            ClearContents();
            this.Parent = parent;
            foreach (var mealData in data)
            {
                VisualElement contentElement = TabContent.CloneTree();
                var button = contentElement.Q(k_ContentButton);
                var highlight = contentElement.Q(k_ContentHighlight);
                var icon = contentElement.Q(k_ContentMealIcon);
                var name = contentElement.Q<Label>(k_ContentMealName);
                var servings = contentElement.Q<Label>(k_ContentMealServings);
                var portionPrice = contentElement.Q<Label>(k_ContentMealPortionPrice);
                var prepPrice = contentElement.Q<Label>(k_ContentMealPrepPrice);
                var prepTime = contentElement.Q<Label>(k_ContentMealPrepTime);
                Content<GameData.MealData> content = new()
                {
                    ContentButton = button,
                    ContentHighlight = highlight,
                    Data = mealData,
                    Locked = Locked,
                };
                content.Locked(content.ContentButton,content.Data);
                Contents.Add(content);
                ContentParent.Add(content.ContentButton);

                button.RegisterCallback((ClickEvent evt) => { SelectContent(content); });
                highlight.style.visibility = Visibility.Hidden;
                icon.style.backgroundImage = new(mealData.meal.Icon);
                name.text = mealData.meal.MealName;
                servings.text = mealData.meal.Servings.ToString();
                portionPrice.text = mealData.meal.PortionPrice.ToString();
                prepPrice.text = mealData.meal.PrepPrice.ToString();
                prepTime.text = mealData.meal.PrepTimeFormatted;
            }
        }
        private bool Locked(VisualElement button, GameData.MealData data)
        {
            var gold = GameDataManager.GameData.gold;
            var priceLock = data.meal.PrepPrice > gold;
            //TODO: show/hide lock visual element
            button.Q<Label>(k_ContentMealPrepPrice).style.color = priceLock ? new(Color.red) : new(Color.white);

            return priceLock;
        }
    }
}
