using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bies.Game.UI
{
    public class ChefSelectionMenu : ScreenBase
    {
        public event Action<CookingOvenBehaviour,ChefBehaviour> OnAssignButtonClick;
        public event Action OnCancelButtonClick;


        //private const string k_BackgroundButton = "background-button";
        private const string k_ContentParent = "content-parent";
        private const string k_AssignButton = "assign-button";
        private const string k_CancelButton = "cancel-button";

        private const string k_ContentButton = "content-button";
        private const string k_ContentHighlight = "content-highlight";
        private const string k_ContentChefIcon = "content-chef-icon";
        private const string k_ContentChefName = "content-chef-name";
        private const string k_ContentChefStatus = "content-chef-status";


        //VisualElement BackgroundButton; //Can add "click anywhere else to cancel functionality" with this
        VisualElement ContentParent;
        VisualElement AssignButton;
        VisualElement CancelButton;

        CookingOvenMenu Parent;
        Content<ChefBehaviour> SelectedContent;
        List<Content<ChefBehaviour>> Contents;
        [SerializeField] VisualTreeAsset TabContent;

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            //BackgroundButton = Root.Q(k_BackgroundButton);
            ContentParent = Root.Q(k_ContentParent);
            AssignButton = Root.Q(k_AssignButton);
            CancelButton = Root.Q(k_CancelButton);

        }
        protected override void RegisterButtonCallbacks()
        {
            AssignButton.RegisterCallback<ClickEvent>(AssignButtonClicked);
            CancelButton.RegisterCallback<ClickEvent>(CancelButtonClicked);
        }
        private void AssignButtonClicked(ClickEvent evt)
        {
            OnAssignButtonClick?.Invoke(Parent.GetData(),SelectedContent?.Data);
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
        private void SelectContent(Content<ChefBehaviour> content)
        {
            SelectedContent = SelectedContent != content ? content: null; //Toggle selection
            UpdateContentHighlights();
        }
        private void UpdateContentHighlights()
        {
            foreach (var content in Contents)
            {
                content.ContentHighlight.style.visibility = (content == SelectedContent) ? Visibility.Visible : Visibility.Hidden;
            }
        }
        public void Populate(List<ChefBehaviour> data, CookingOvenMenu parent)
        {
            ClearContents();
            this.Parent = parent;
            foreach (var chef in data)
            {
                VisualElement contentElement = TabContent.CloneTree();
                var button = contentElement.Q(k_ContentButton);
                var highlight = contentElement.Q(k_ContentHighlight);
                var icon = contentElement.Q(k_ContentChefIcon);
                var name = contentElement.Q<Label>(k_ContentChefName);
                var status = contentElement.Q<Label>(k_ContentChefStatus);
                Content<ChefBehaviour> content = new()
                {
                    ContentButton = button,
                    ContentHighlight = highlight,
                    Data = chef,
                };
                Contents.Add(content);
                ContentParent.Add(content.ContentButton);

                button.RegisterCallback((ClickEvent evt) => { SelectContent(content); });
                highlight.style.visibility = Visibility.Hidden;
                icon.style.backgroundImage = new(chef.Icon);
                name.text = chef.Name;
                status.text = chef.GetStatus();

                if (chef == parent.GetData().assignedChef) SelectContent(content);
            }
        }
    }
}
