using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bies.Game.UI
{
    public class HudBottom : ScreenBase
    {
        public event Action ConstructionButtonClicked;
        public event Action CookBookButtonClicked;
        public event Action ShopButtonClicked;
        public event Action HiringButtonClicked;

        const string k_ConstructionButton = "construction-button";
        const string k_CookBookButton = "cook-book-button";
        const string k_ShopButton = "shop-button";
        const string k_HiringButton = "hiring-button";


        VisualElement ConstructionButton;
        VisualElement CookBookButton;
        VisualElement ShopButton;
        VisualElement HiringButton;

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            ConstructionButton = Root.Q(k_ConstructionButton);
            CookBookButton = Root.Q(k_CookBookButton);
            ShopButton = Root.Q(k_ShopButton);
            HiringButton = Root.Q(k_HiringButton);
        }
        protected override void RegisterButtonCallbacks()
        {
            ConstructionButton.RegisterCallback<ClickEvent>(ConstructionButtonClick);
            CookBookButton.RegisterCallback<ClickEvent>(CookBookButtonClick);
            ShopButton.RegisterCallback<ClickEvent>(ShopButtonClick);
            HiringButton.RegisterCallback<ClickEvent>(HiringButtonClick);
        }

        void ConstructionButtonClick(ClickEvent evt)
        {
            ConstructionButtonClicked?.Invoke();
        }
        void CookBookButtonClick(ClickEvent evt)
        {
            CookBookButtonClicked?.Invoke();
        }
        void ShopButtonClick(ClickEvent evt)
        {
            ShopButtonClicked?.Invoke();
        }
        void HiringButtonClick(ClickEvent evt)
        {
            HiringButtonClicked?.Invoke();
        }
    }
}
