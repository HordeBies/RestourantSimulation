using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bies.Game.UI
{
    public class HiringMenu : ScreenBase
    {
        public event Action OnBackgroundButtonClick;
        public event Action<GameData.WorkerData> OnHireButtonClick;

        private const string k_BackgroundButton = "background-button";
        private const string k_ContentParent = "content-parent";

        private const string k_ChefTab = "chef-tab";
        private const string k_ChefHighlight = "chef-highlight";
        private const string k_ServerTab = "server-tab";
        private const string k_ServerHighlight = "server-highlight";

        private const string k_ContentButton = "content-button";
        private const string k_ContentIcon = "content-icon";
        private const string k_ContentName = "content-name";
        private const string k_ContentPrice = "content-price";
        private const string k_ContentHireParent = "content-hire-parent";
        private const string k_ContentHireButton = "hire-button";
        private const string k_ContentPriceLock = "content-price-lock";
        private const string k_ContentLevelLock = "content-level-lock";
        private const string k_ContentLevelLabel = "content-level-label";

        VisualElement BackgroundButton;
        VisualElement ContentParent;
        [SerializeField] VisualTreeAsset TabContent;

        TabControl<GameData.WorkerData> TabControl;
        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            
            BackgroundButton = Root.Q(k_BackgroundButton);
            ContentParent = Root.Q(k_ContentParent);

            TabControl = new();
            TabControl.RegisterTab(Root.Q(k_ChefTab), Root.Q(k_ChefHighlight) , ContentParent, GameDataManager.GameData.ChefData, PopulateTab);
            TabControl.RegisterTab(Root.Q(k_ServerTab), Root.Q(k_ServerHighlight), ContentParent, GameDataManager.GameData.ServerData, PopulateTab);
            TabControl.SelectFirstTab();
        }
        private void PopulateTab(List<GameData.WorkerData> data)
        {
            Debug.Log("Trying to populate a tab!");
            TabControl.ClearContents();
            foreach (var item in data)
            {
                VisualElement content = TabContent.CloneTree();
                var button = content.Q(k_ContentButton);

                var icon = content.Q(k_ContentIcon);
                icon.style.backgroundImage = new StyleBackground(item.worker.icon);
                var name = content.Q<Label>(k_ContentName);
                name.text = item.worker.nameString;
                var price = content.Q<Label>(k_ContentPrice);
                price.text = item.worker.price.ToString();
                var hire = content.Q(k_ContentHireButton);
                hire.RegisterCallback<ClickEvent>((evt) => { OnHireButtonClick?.Invoke(item); });
                var level = content.Q<Label>(k_ContentLevelLabel);
                level.text = item.worker.unlockedLevel.ToString();

                TabControl.RegisterContent(button, null, item, Locked, false);
            }

        }
        private bool Locked(VisualElement button, GameData.WorkerData data)
        {
            var gold = GameDataManager.GameData.gold;
            var level = GameDataManager.GameData.level;
            var hired = data.isHired;
            ShowVisualElement(button.Q(k_ContentHireParent), !hired);
            var levelLocked = data.worker.unlockedLevel > level;
            ShowVisualElement(button.Q(k_ContentLevelLock), levelLocked);
            var priceLocked = data.worker.price > gold;
            button.Q<Label>(k_ContentPrice).style.color = priceLocked ? new(Color.red) : new(Color.white);
            ShowVisualElement(button.Q(k_ContentPriceLock), priceLocked);
            return hired || levelLocked || priceLocked;
        }
        protected override void RegisterButtonCallbacks()
        {
            BackgroundButton.RegisterCallback<ClickEvent>(BackgroundButtonClick);
        }
        private void BackgroundButtonClick(ClickEvent evt)
        {
            OnBackgroundButtonClick?.Invoke();
        }
        public override void ShowScreen()
        {
            TabControl.RefreshTab();
            base.ShowScreen();
        }
        public override void HideScreen()
        {
            base.HideScreen();
            if(TabControl != null)
                TabControl.SelectFirstTab();
        }
    }
}
