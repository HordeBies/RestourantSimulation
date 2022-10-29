using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bies.Game.UI
{
    public class ConstructionMenu : ScreenBase
    {
        public event Action OnDoneButtonClicked;
        public event Action<GridObject> OnGridObjectSelected;


        private const string k_TabButtonSuffix = "-tab";
        private const string k_HighlightSuffix = "-highlight";

        private const string k_DiningTable = "dining-table";
        private const string k_Chair = "chair";
        private const string k_Door = "door";
        private const string k_FloorTile = "floor-tile";
        private const string k_Wallpaper = "wallpaper";
        private const string k_ServingTable = "serving-table";
        private const string k_Oven = "oven";

        private const string k_TabContent = "tab-content";
        private const string k_DoneButton = "done-button";

        private const string k_ContentButton = "content-button";
        private const string k_ContentHighlight = "content-highlight";
        private const string k_ContentIcon = "content-icon";
        //private const string k_ContentCount = "content-count";
        private const string k_ContentPrice = "content-price";
        private const string k_ContentLevelLock = "content-level-lock";
        private const string k_ContentLevelLabel = "content-level-label";
        private const string k_ContentPriceLock = "content-price-lock";

        VisualElement DoneButton;
        VisualElement TabContentParent;
        [SerializeField] VisualTreeAsset TabContent;

        TabControl<GridObject> TabControl;
        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            DoneButton = Root.Q(k_DoneButton);
            TabContentParent = Root.Q(k_TabContent);
            
            TabControl = new();
            TabControl.OnSelectedContentChanged += SelectGridObject;
            TabControl.RegisterTab(Root.Q(k_DiningTable + k_TabButtonSuffix), Root.Q(k_DiningTable + k_HighlightSuffix), TabContentParent, GameDataManager.GameData.Storage.diningTables, PopulateConstructionTab);
            TabControl.RegisterTab(Root.Q(k_Chair + k_TabButtonSuffix), Root.Q(k_Chair + k_HighlightSuffix), TabContentParent, GameDataManager.GameData.Storage.chairs, PopulateConstructionTab);
            TabControl.RegisterTab(Root.Q(k_Door + k_TabButtonSuffix), Root.Q(k_Door + k_HighlightSuffix), TabContentParent, GameDataManager.GameData.Storage.doors, PopulateConstructionTab);
            TabControl.RegisterTab(Root.Q(k_FloorTile + k_TabButtonSuffix), Root.Q(k_FloorTile + k_HighlightSuffix), TabContentParent, GameDataManager.GameData.Storage.floorTiles, PopulateConstructionTab);
            TabControl.RegisterTab(Root.Q(k_Wallpaper + k_TabButtonSuffix), Root.Q(k_Wallpaper + k_HighlightSuffix), TabContentParent, GameDataManager.GameData.Storage.wallPapers, PopulateConstructionTab);
            TabControl.RegisterTab(Root.Q(k_ServingTable + k_TabButtonSuffix), Root.Q(k_ServingTable + k_HighlightSuffix), TabContentParent, GameDataManager.GameData.Storage.servingTables, PopulateConstructionTab);
            TabControl.RegisterTab(Root.Q(k_Oven + k_TabButtonSuffix), Root.Q(k_Oven + k_HighlightSuffix), TabContentParent, GameDataManager.GameData.Storage.cookingOvens, PopulateConstructionTab);
            TabControl.SelectFirstTab();
        }
        private void OnEnable()
        {
            ConstructionManager.OnObjectPlaced += RefreshTab;
            ConstructionManager.OnObjectRemoved+= RefreshTab;
        }
        private void PopulateConstructionTab(List<GridObject> data)
        {
            Debug.Log("Trying to populate a tab!");
            TabControl.ClearContents();
            foreach (var item in data)
            {
                VisualElement content = TabContent.CloneTree();
                var button = content.Q(k_ContentButton);
                var highlight = content.Q(k_ContentHighlight);

                var icon = content.Q(k_ContentIcon);
                icon.style.backgroundImage = new StyleBackground(item.icon);
                var price = content.Q<Label>(k_ContentPrice);
                price.text = item.price.ToString();
                var level = content.Q<Label>(k_ContentLevelLabel);
                level.text = item.unlockedLevel.ToString();

                TabControl.RegisterContent(button,highlight,item,Locked);
            }
        }
        private void RefreshTab(PlacedObject _)
        {
            TabControl.RefreshTab();
        }
        private bool Locked(VisualElement button, GridObject data)
        {
            var gold = GameDataManager.GameData.gold;
            var level = GameDataManager.GameData.level;

            var price = button.Q<Label>(k_ContentPrice);
            price.style.color = new(data.price > gold ? Color.red : Color.white);
            var priceLock = button.Q(k_ContentPriceLock);
            ShowVisualElement(priceLock, data.price > gold);
            var levelLock = button.Q(k_ContentLevelLock);
            ShowVisualElement(levelLock, data.unlockedLevel > level);
            return data.price > gold || data.unlockedLevel > level;
        }
        protected override void RegisterButtonCallbacks()
        {
            DoneButton.RegisterCallback<ClickEvent>(DoneButtonClick);
        }
        private void SelectGridObject(GridObject clickedObj)
        {
            OnGridObjectSelected?.Invoke(clickedObj);
        }
        private void DoneButtonClick(ClickEvent evt)
        {
            OnDoneButtonClicked?.Invoke();
        }
        public override void HideScreen()
        {
            base.HideScreen();
            if(TabControl != null)
            {
                TabControl.ClearSelectedContent();
                TabControl.SelectFirstTab();
            }
        }
    }
}
