using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Bies.Game;
using Bies.Game.UI;

public enum GameState
{
    Default,
    Construction,
}
[RequireComponent(typeof(PlayerInput))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool PointerOverUI { private set; get; }

    public GameState gameState = GameState.Default;
    public bool showGridDebug;

    private GameUIManager uiManager => GameUIManager.instance;
    private GameDataManager gameDataManager => GameDataManager.instance;
    private PlayerInput playerInput;

    private void Awake()
    {
        instance = this;
        playerInput = GetComponent<PlayerInput>();
        CafeSimulationManager.GetDoorTile = () => CafeGridTile.DoorTile;
        
    }
    private void FixedUpdate()
    {
        PointerOverUI = EventSystem.current.IsPointerOverGameObject();
    }
    public void SwitchToConstructionMode()
    {
        playerInput.SwitchCurrentActionMap("Construction");
        gameState = GameState.Construction;
        CafeSimulationManager.instance.HideNPCs();
        uiManager.ShowConstructionMenu();
    }

    public void SwitchToDefaultMode()
    {
        playerInput.SwitchCurrentActionMap("Default");
        gameState = GameState.Default;
        CafeSimulationManager.instance.ShowNPCs();
        uiManager.ShowHudBottom();
    }

    #region UI Bindings
    public void ConstructionManager_SelectGridObject(GridObject selected)
    {
        ConstructionManager.instance.SetSelectedGridObject(selected);
    }
    public void CookingOvenMenu_SelectMeal(CookingOvenMenu co)
    {
        if(co.GetData().meal == null)
        {
            uiManager.ShowMenu(gameDataManager.gameData.CookBookData.FindAll(i => i.isLearned),co);
        }
        else
        {
            //TODO: Show Quick Prep Pop-Up
        }
    }
    public void CookingOvenMenu_SelectChef(CookingOvenMenu co)
    {
        uiManager.ShowMenu(CafeSimulationManager.instance.Chefs,co);
    }
    public void ServingTableMenu_ClearServingTable(ServingTableMenu st)
    {
        st.GetData().ClearTable();
    }
    public void ChefSelectionMenu_AssignNewChef(CookingOvenBehaviour co, ChefBehaviour chef)
    {
        co.AssignChef(chef);
        uiManager.HideChefSelectionMenu();
    }
    public void MealSelectionMenu_PrepareMeal(CookingOvenBehaviour co, Meal meal)
    {
        co.Cook(meal);
        gameDataManager.PurchaseMeal(meal);
        uiManager.HideMealSelectionMenu();
    }

    public void HiringMenu_HireWorker(GameData.WorkerData data)
    {
        ConstructionManager.instance.SpawnAtDoor(data.worker);
        gameDataManager.HireWorker(data);
        Debug.Log("Hired " + data.worker.ToString());
    }
    #endregion

    #region DatabaseBindings
    public GridObject GetRandomCustomer()
    {
        return Database.Customers[Random.Range(0,Database.Customers.Count)];
    }
    #endregion

}
