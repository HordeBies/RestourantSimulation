using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    public static GameData GameData => instance.gameData;

    public static event Action<GameData> OnFundsUpdated;

    public GameData gameData;

    SessionManager sessionManager;
    bool isGameDataInitialized = false;

    private void Awake()
    {
        instance = this;
        gameData.Populate();
    }
    private void OnEnable()
    {
        ConstructionManager.OnObjectPlaced += PlaceObject;
        ConstructionManager.OnObjectRemoved += RemoveObject;
    }
    public void PlaceObject(PlacedObject obj)
    {
        bool load = GameManager.instance.gameState != GameState.Construction;
        var dir = obj.dir;
        var pos = obj.origin;
        var behaviour = obj.GetComponent<BaseBehaviour>();
        if(behaviour is NPCBehaviour)
        {
            if (behaviour is ChefBehaviour)
                gameData.ChefData.Find(i => i.worker == behaviour.GetData()).behaviour = (NPCBehaviour)behaviour;
            if(behaviour is ServerBehaviour)
                gameData.ServerData.Find(i => i.worker == behaviour.GetData()).behaviour = (NPCBehaviour)behaviour;

            return;
        }

        if (!load)
        {
            var storage = gameData.Storage.Find(obj.placedObjectTypeSO);
            BuyObject(storage);
        }

        gameData.CafePlacedObjects.Add(new() { Data = obj, dir = dir, pos = pos, behaviour = behaviour });
    }
    public void BuyObject(GridObject obj)
    {
        gameData.gold -= obj.price;
        OnFundsUpdated?.Invoke(gameData);
    }
    public void SellObject(GridObject obj)
    {
        gameData.gold += obj.price;
        OnFundsUpdated?.Invoke(gameData);
    }
    public void RemoveObject(PlacedObject obj)
    {
        var haveBehaviour = obj.TryGetComponent<BaseBehaviour>(out var behaviour);
        if (haveBehaviour && behaviour is NPCBehaviour) return;
        var count = gameData.CafePlacedObjects.RemoveAll(i => i.Data == obj);
        SellObject(obj.placedObjectTypeSO);
        Debug.Log($"Removed {count} object!");
    }
    public void PurchaseMeal(Meal meal)
    {
        gameData.gold -= meal.PrepPrice;
        OnFundsUpdated?.Invoke(gameData);
    }
    public void HireWorker(GameData.WorkerData hired)
    {
        gameData.gold -= hired.worker.price;
        OnFundsUpdated?.Invoke(gameData);
        hired.isHired = true;
    }
    public void GainExperience(int amount)
    {
        gameData.exp += amount;
        var reqExp = Database.RequiredExp(gameData.level);//Can change it to do while, if more than 1 level upgrade is intented or possible
        if (gameData.exp > reqExp)
        {
            gameData.level++;
        }
    }
    public void UpdateFunds()
    {
        OnFundsUpdated?.Invoke(gameData);
    }
}
