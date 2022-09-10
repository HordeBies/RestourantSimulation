using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CafeSimulationManager : MonoBehaviour
{
    public static CafeSimulationManager instance;
    public static Func<PlacedObject> GetDoorTile;

    private List<DiningTableBehaviour> DiningTables = new();
    private List<PlacedObject> ServingTables = new();
    private List<ChairBehaviour> Chairs = new();

    private List<CustomerBehaviour> Customers = new();
    private List<PlacedObject> Servers = new();
    private List<PlacedObject> Chefs = new();


    private void Awake()
    {
        instance = this;
        ConstructionManager.OnObjectPlaced += AddNewObject;
    }

    public void SpawnRandomCustomer() //Instead of returning spawned object, if anything goes wrong, i call an event on successfull spawn
    {
        var doorTile = GetDoorTile?.Invoke();
        ConstructionManager.instance.Spawn(doorTile.origin, GridObject.GetReverseDir(doorTile.dir), GameManager.instance.GetRandomCustomer());
    }
    public bool AssignSeat(CustomerBehaviour customer)
    {
        var chair = Chairs.FirstOrDefault(chair => chair.IsAvailable());
        if (chair == null) return false;

        bool canReach = false;
        GridObject.Dir emptySide = default;
        foreach (var dir in GridObject.Directions)
        {
            var obj = ConstructionManager.instance.GetPlacedObject(chair.placedObject.GetAdjacentTilePos(dir));
            if (obj == null)
            {
                canReach = true;
                emptySide = dir;
                break;
            }
        }
        if (!canReach) return false;
        //var emptySide = GridObject.Directions.FirstOrDefault(dir => ConstructionManager.instance.GetPlacedObject(chair.placedObject.GetAdjacentTilePos(dir)) == null);
        chair.Assign(customer);
        customer.Move(ConstructionManager.instance.GetBorder(chair.placedObject, emptySide));
        return true;
    }
    //Caching simulation rules here
    private void AddNewObject(PlacedObject obj)
    {
        switch (obj.Type)
        {
            case ObjectType.NaN:
                Debug.LogError("NAN Object Type Spawned!");
                break;
            case ObjectType.Chair:
                UpdateChair(obj,out ChairBehaviour chair);
                Chairs.Add(chair);
                break;
            case ObjectType.CookingOven:
                break;
            case ObjectType.DiningTable:
                var diningTable = obj.GetComponent<DiningTableBehaviour>();
                foreach (var dir in GridObject.Directions)
                {
                    var potentialChair = ConstructionManager.instance.GetPlacedObject(obj.GetAdjacentTilePos(dir));
                    if (potentialChair != null && potentialChair.Type == ObjectType.Chair) UpdateChair(potentialChair, out _);
                }
                DiningTables.Add(diningTable);
                break;
            case ObjectType.FloorTile:
                break;
            case ObjectType.ServingTable:
                ServingTables.Add(obj);
                break;
            case ObjectType.Wallpaper:
                break;
            case ObjectType.Customer:
                var customer = obj.GetComponent<CustomerBehaviour>();
                Customers.Add(customer);
                break;
            case ObjectType.Chef:
                Chefs.Add(obj);
                break;
            case ObjectType.Server:
                Servers.Add(obj);
                break;
            default:
                break;
        }
    }
    private void UpdateChair(PlacedObject obj, out ChairBehaviour chair)
    {
        chair = obj.GetComponent<ChairBehaviour>();
        var frontObj = ConstructionManager.instance.GetPlacedObject(obj.GetFrontTilePos());
        if (frontObj.Type == ObjectType.DiningTable)
            chair.SetDiningTable(frontObj.GetComponent<DiningTableBehaviour>());
    }

    public void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || GameManager.PointerOverUI) return;

        var go = Mouse3D.GetClickedGameObject();
        if(go != null && go.TryGetComponent<BaseBehaviour>(out var baseBehaviour))
        {
            baseBehaviour.OnClick();
            var gridObject = baseBehaviour.GetData();
            Debug.Log(gridObject.ToString());
        }
    }

}
