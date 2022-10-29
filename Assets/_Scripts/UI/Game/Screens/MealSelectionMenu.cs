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
        private const string k_ContentMealIcon = "content-meal-icon";
        private const string k_ContentMealName = "content-meal-name";
        private const string k_ContentMealPrice = "content-meal-price";


        //VisualElement BackgroundButton; //Can add "click anywhere else to cancel functionality" with this
        VisualElement ContentParent;
        VisualElement PrepareButton;
        VisualElement CancelButton;

        CookingOvenMenu Parent;
        Content<Meal> SelectedContent;
        List<Content<Meal>> Contents;
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
            OnPrepareButtonClick?.Invoke(Parent.GetData(),SelectedContent.Data);
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
        private void SelectContent(Content<Meal> content)
        {
            if (SelectedContent == content) return;
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
        public void Populate(List<Meal> data, CookingOvenMenu parent)
        {
            ClearContents();
            this.Parent = parent;
            foreach (var meal in data)
            {
                VisualElement contentElement = TabContent.CloneTree();
                var button = contentElement.Q(k_ContentButton);
                var highlight = contentElement.Q(k_ContentHighlight);
                var icon = contentElement.Q(k_ContentMealIcon);
                var name = contentElement.Q<Label>(k_ContentMealName);
                var price = contentElement.Q<Label>(k_ContentMealPrice);
                Content<Meal> content = new()
                {
                    ContentButton = button,
                    ContentHighlight = highlight,
                    Data = meal,
                    Locked = Locked,
                };
                Contents.Add(content);
                ContentParent.Add(content.ContentButton);

                button.RegisterCallback((ClickEvent evt) => { SelectContent(content); });
                highlight.style.visibility = Visibility.Hidden;
                icon.style.backgroundImage = new(meal.Icon);
                name.text = meal.MealName;
                price.text = meal.Servings.ToString();//TODO CREATE PRICE TAG
            }
        }
        private bool Locked(VisualElement button, Meal data)
        {
            //TODO: Handle lock
            return false;
        }
    }
}
