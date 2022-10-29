using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bies.Game.UI
{
    public class HudTop : ScreenBase
    {
        const string k_OptionsButton = "options-button";
        const string k_ShopGemButton = "gem-button";
        const string k_ShopGoldButton = "gold-button";
        const string k_RestourantNameButton = "restourant-name-button";


        const string k_GemCount = "gem-count";
        const string k_GoldCount = "gold-count";
        const string k_RestourantName = "restourant-name";
        const string k_RestourantLevel = "restourant-level";
        const string k_RestourantLevelParent = "restourant-level-parent";

        VisualElement OptionsButton;
        VisualElement ShopGemButton;
        VisualElement ShopGoldButton;
        VisualElement RestourantNameButton;
        VisualElement RestourantLevelParent;
        Label GoldLabel;
        Label GemLabel;
        Label RestourantName;
        Label RestourantLevel;

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            OptionsButton = Root.Q(k_OptionsButton);
            ShopGoldButton = Root.Q(k_ShopGoldButton);
            ShopGemButton = Root.Q(k_ShopGemButton);
            RestourantNameButton = Root.Q(k_RestourantNameButton);
            RestourantLevelParent = Root.Q(k_RestourantLevelParent);

            // shorthand equivalent to getting the first item named m_ShopGemButtonName:
            //      ShopGemButton = rootElement.Query<VisualElement>(m_ShopGemButtonName).First();

            GoldLabel = Root.Q<Label>(k_GoldCount);
            GemLabel = Root.Q<Label>(k_GemCount);
            RestourantName = Root.Q<Label>(k_RestourantName);
            RestourantLevel = Root.Q<Label>(k_RestourantLevel);
        }
        private void OnEnable()
        {
            GameDataManager.OnFundsUpdated += UpdateHUD;
            UpdateHUD(GameDataManager.GameData);
        }
        private void UpdateHUD(GameData data)
        {
            GoldLabel.text = data.gold.ToString();
            GemLabel.text = data.gems.ToString();
            RestourantName.text = data.username;
            RestourantLevel.text = data.level.ToString();
        }
        protected override void RegisterButtonCallbacks()
        {
            OptionsButton?.RegisterCallback<ClickEvent>(ShowOptionsScreen);
            ShopGemButton?.RegisterCallback<ClickEvent>(OpenGemShop);
            ShopGoldButton?.RegisterCallback<ClickEvent>(OpenGoldShop);
            RestourantNameButton?.RegisterCallback<ClickEvent>(OpenRestourantNameChanger);
        }

        void ShowOptionsScreen(ClickEvent evt)
        {
            Debug.Log("ShowOptionsScreen");
        }
        void OpenGemShop(ClickEvent evt)
        {
            Debug.Log("OpenGemShop");
        }
        void OpenGoldShop(ClickEvent evt)
        {
            Debug.Log("OpenGoldShop");
        }
        void OpenRestourantNameChanger(ClickEvent evt)
        {
            Debug.Log("RestourantName: " +RestourantName.text);
        }

        public void HideLevelBar()
        {
            ShowVisualElement(RestourantLevelParent, false);
        }
        public void ShowLevelBar()
        {
            ShowVisualElement(RestourantLevelParent, true);
        }
    }
}
