using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bies.Game.UI
{
    public class Content<T>
    {
        public VisualElement ContentButton;
        public VisualElement ContentHighlight;
        public Func<VisualElement,T,bool> Locked; //Used to refresh data&ui binding then returns lock status
        public T Data;
    }
    public class TabControl<T>
    {
        public event Action OnSelectedTabChanged;
        public event Action<T> OnSelectedContentChanged;

        private List<Tab<T>> Tabs = new();


        private Tab<T> SelectedTab;
        public Content<T> SelectedContent;

        public class Tab<T>
        {
            public VisualElement TabButton;
            public VisualElement TabHighlight;
            public VisualElement TabContent;
            public List<T> DataBinding;
            public Action<List<T>> Populate;

            public List<Content<T>> Contents;
        }

        public TabControl()
        {

        }
        public void RegisterTab(VisualElement tabButton,VisualElement highlight, VisualElement content, List<T> data, Action<List<T>> populate)
        {
            Tab<T> tab = new()
            {
                TabButton = tabButton,
                TabHighlight = highlight,
                TabContent = content,
                DataBinding = data,
                Populate = populate,
            };
            Tabs.Add(tab);
            if(highlight != null) highlight.style.visibility = Visibility.Hidden;
            tabButton.RegisterCallback<ClickEvent>((ClickEvent evt) => { SelectTab(tab); } );
        }
        public void RegisterContent(VisualElement contentButton, VisualElement contentHighlight, T data, Func<VisualElement,T,bool> LockBehaviour, bool selectable = true)
        {//TODO: Return created content
            Content<T> content = new() 
            { 
                ContentButton = contentButton, 
                ContentHighlight = contentHighlight, 
                Data = data,
                Locked = LockBehaviour,
            };
            content.Locked(content.ContentButton, content.Data);
            SelectedTab.Contents.Add(content);
            SelectedTab.TabContent.Add(contentButton);

            if(contentHighlight != null) contentHighlight.style.visibility = Visibility.Hidden;
            if (selectable) contentButton.RegisterCallback<ClickEvent>((ClickEvent evt) => { SelectContent(content); });
        }
        public void ClearContents()
        {
            SelectedTab.TabContent.Clear();
            SelectedTab.Contents = new();
        }
        public void SelectFirstTab()
        {
            if (Tabs == null || Tabs.Count < 1) return;
            SelectTab(Tabs[0]);
        }
        public void ClearSelectedContent()
        {
            SelectContent(null);
        }
        public void RefreshTab()
        {
            SelectedTab.Contents.ForEach(i => i.Locked(i.ContentButton, i.Data));
            if (SelectedContent != null && SelectedContent.Locked(SelectedContent.ContentButton, SelectedContent.Data)) ClearSelectedContent();
        }
        public void SelectTab(Tab<T> tab)
        {
            if (SelectedTab == tab) return;
            SelectedTab = tab;

            UpdateTabHighlights();
            SelectedTab.Populate?.Invoke(SelectedTab.DataBinding);
            RefreshContent();
            OnSelectedTabChanged?.Invoke();
        }
        private void RefreshContent() //Used for reloading previously selected content
        {
            if (SelectedContent == null || SelectedTab == null || SelectedTab.Contents == null) return;
            var content = SelectedTab.Contents.FirstOrDefault(i => i.Data.Equals(SelectedContent.Data));
            if (content != null) SelectContent(content);
        }
        public void SelectContent(Content<T> content)
        {
            if (content != null && content.Locked(content.ContentButton,content.Data)) return;
            if (SelectedContent == content) return;
            SelectedContent = content;

            UpdateContentHighlights();
            T data = content != null ? content.Data : default(T);
            OnSelectedContentChanged?.Invoke(data);
        }
        private void UpdateTabHighlights()
        {
            foreach (var tab in Tabs)
            {
                if(tab.TabHighlight != null)
                    tab.TabHighlight.style.visibility = (tab == SelectedTab) ? Visibility.Visible : Visibility.Hidden;
            }
        }
        private void UpdateContentHighlights()
        {
            foreach (var content in SelectedTab.Contents)
            {
                if(content.ContentHighlight != null)
                    content.ContentHighlight.style.visibility = (content == SelectedContent) ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }

}
