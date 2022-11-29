using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bies.Game.UI;
namespace Bies.Game
{
    public class GameUIManager : MonoBehaviour
    {
        public static GameUIManager instance;

        [Header("Toolbars")]
        [Tooltip("Toolbars remain active at all times unless explicitly disabled.")]
        [SerializeField] HudTop hudTop;
        [Space]
        [Header("Full-screen overlays")]
        [Tooltip("Full-screen overlays block other controls until dismissed.")]
        [SerializeField] CookingOvenMenu cookingOvenMenu;
        [SerializeField] ChefSelectionMenu chefSelectionMenu;
        [SerializeField] MealSelectionMenu mealSelectionMenu;
        [SerializeField] ServingTableMenu servingTableMenu;
        [SerializeField] HiringMenu hiringMenu;
        [Space]
        [Header("Modal Menu Screens")]
        [Tooltip("Only one modal interface can appear on-screen at a time.")]
        [SerializeField] HudBottom hudBottom;
        [SerializeField] ConstructionMenu constructionMenu;

        private GameManager Manager => GameManager.instance;
        private List<ScreenBase> ModalScreens;

        private void Awake()
        {
            instance = this;
        }
        private void OnEnable()
        {
            hudTop.ShowScreen();
            hudBottom.ShowScreen();

            cookingOvenMenu.HideScreen();
            servingTableMenu.HideScreen();
            chefSelectionMenu.HideScreen();
            mealSelectionMenu.HideScreen();
            hiringMenu.HideScreen();

            SetupModalScreens();
            ShowHudBottom();

            #region Events
            //HudBottom
            hudBottom.ConstructionButtonClicked += Manager.SwitchToConstructionMode;
            hudBottom.CookBookButtonClicked += HudBottom_CookBookButtonClicked;
            hudBottom.ShopButtonClicked += HudBottom_ShopButtonClicked;
            hudBottom.HiringButtonClicked += HudBottom_HiringButtonClicked;

            //ConstructionMenu
            constructionMenu.OnDoneButtonClicked += Manager.SwitchToDefaultMode;
            constructionMenu.OnGridObjectSelected += Manager.ConstructionManager_SelectGridObject;

            //CookingOvenMenu
            cookingOvenMenu.OnBackgroundButtonClick += HideCookingOvenMenu;
            cookingOvenMenu.OnChefButtonClick += Manager.CookingOvenMenu_SelectChef;
            cookingOvenMenu.OnMealButtonClick += Manager.CookingOvenMenu_SelectMeal;

            //ServingTableMenu
            servingTableMenu.OnBackgroundButtonClick += HideServingTableMenu;
            servingTableMenu.OnClearButtonClick += Manager.ServingTableMenu_ClearServingTable;

            //ChefSelectionMenu
            chefSelectionMenu.OnAssignButtonClick += Manager.ChefSelectionMenu_AssignNewChef;
            chefSelectionMenu.OnCancelButtonClick += HideChefSelectionMenu;

            //MealSelectionMenu
            mealSelectionMenu.OnPrepareButtonClick += Manager.MealSelectionMenu_PrepareMeal;
            mealSelectionMenu.OnCancelButtonClick += HideMealSelectionMenu;

            //HiringMenu
            hiringMenu.OnBackgroundButtonClick += HideHiringMenu;
            hiringMenu.OnHireButtonClick += Manager.HiringMenu_HireWorker;
            #endregion
        }
        private void HudBottom_ShopButtonClicked()
        {
            Debug.Log("HudBottom_ShopButtonClicked");
        }

        private void HudBottom_CookBookButtonClicked()
        {
            Debug.Log("HudBottom_CookBookButtonClicked");
        }
        private void HudBottom_HiringButtonClicked()
        {
            ShowHiringMenu();
        }
        private void Start()
        {
            Time.timeScale = 1f;
        }
        private void SetupModalScreens()
        {
            ModalScreens = new();
            if (hudBottom != null) ModalScreens.Add(hudBottom);
            if (constructionMenu != null) ModalScreens.Add(constructionMenu);
        }
        private void ShowModalScreen(ScreenBase modalScreen)
        {
            foreach (var m in ModalScreens)
            {
                if (m == modalScreen)
                {
                    m?.ShowScreen();
                }
                else
                {
                    m?.HideScreen();
                }
            }
        }
        public void ShowHudBottom()
        {
            ShowModalScreen(hudBottom);
        }
        public void ShowConstructionMenu()
        {
            ShowModalScreen(constructionMenu);
        }
        public void ShowHiringMenu()
        {
            hiringMenu.ShowScreen();
        }
        public void ShowMenu(CookingOvenBehaviour data)
        {
            cookingOvenMenu.Populate(data);
            cookingOvenMenu.ShowScreen();
        }
        public void ShowMenu(ServingTableBehaviour data)
        {
            servingTableMenu.Populate(data);
            servingTableMenu.ShowScreen();
        }
        public void ShowMenu(List<GameData.MealData> data, CookingOvenMenu parent)
        {
            mealSelectionMenu.Populate(data, parent);
            mealSelectionMenu.ShowScreen();
        }
        public void ShowMenu(List<ChefBehaviour> data,CookingOvenMenu parent)
        {
            chefSelectionMenu.Populate(data, parent);
            chefSelectionMenu.ShowScreen();
        }
        public void HideCookingOvenMenu()
        {
            //TODO: clear data
            cookingOvenMenu.HideScreen();
        }
        public void HideServingTableMenu()
        {
            //TODO: clear data
            servingTableMenu.HideScreen();
        }
        public void HideMealSelectionMenu()
        {
            //TODO: clear data
            mealSelectionMenu.HideScreen();
        }
        public void HideChefSelectionMenu()
        {
            //TODO: clear data
            chefSelectionMenu.HideScreen();
        }
        public void HideHiringMenu()
        {
            //TODO: clear data
            hiringMenu.HideScreen();
        }
    }
}
