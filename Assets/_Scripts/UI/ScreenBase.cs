using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bies.Game.UI
{
    public abstract class ScreenBase : MonoBehaviour
    {
        [SerializeField] protected string ScreenName;
        [Header("UI Management")]
        [SerializeField] protected UIDocument Document;

        // visual elements
        protected VisualElement Screen;
        protected VisualElement Root;

        public event Action ScreenStarted;
        public event Action ScreenEnded;

        protected virtual void Awake()
        {
            if (Document == null)
            {
                Debug.LogWarning("MenuScreen " + ScreenName + ": missing UIDocument. Check Script Execution Order.");
                return;
            }
            else
            {
                SetVisualElements();
                RegisterButtonCallbacks();
            }
        }
        protected virtual void SetVisualElements()
        {
            // get a reference to the root VisualElement 
            if (Document != null)
                Root = Document.rootVisualElement;

            Screen = Root;
            ShowVisualElement(Screen, true);
        }

        protected virtual void RegisterButtonCallbacks()
        {

        }

        public bool IsVisible()
        {
            if (Screen == null)
                return false;

            return (Screen.style.display == DisplayStyle.Flex);
        }

        // Toggle a UI on and off using the DisplayStyle. 
        public static void ShowVisualElement(VisualElement visualElement, bool state)
        {
            if (visualElement == null)
                return;

            visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
        }

        // returns an element by name
        public VisualElement GetVisualElement(string elementName)
        {
            if (string.IsNullOrEmpty(elementName) || Root == null)
                return null;

            // query and return the element
            return Root.Q(elementName);
        }

        public virtual void ShowScreen()
        {
            ShowVisualElement(Screen, true);
            ScreenStarted?.Invoke();
        }

        public virtual void HideScreen()
        {
            if (IsVisible())
            {
                ShowVisualElement(Screen, false);
                ScreenEnded?.Invoke();
            }
        }
    }
}
