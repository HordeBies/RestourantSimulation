using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CafeSimulationManager : MonoBehaviour
{
    public static CafeSimulationManager instance;
    public static Func<PlacedObject> GetDoorTile;
    public SimulationPanel ui => SimulationPanel.instance;

    public Queue<ServerJob> ServerJobs = new();

    private List<DiningTableBehaviour> DiningTables = new();
    private List<ServingTableBehaviour> ServingTables = new();
    private List<ChairBehaviour> Chairs = new();
    private List<CookingOvenBehaviour> CookingOvens = new();

    private List<CustomerBehaviour> Customers = new();
    private List<PlacedObject> Servers = new();
    public List<ChefBehaviour> Chefs { private set; get; } = new();


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
    public void PrepareRandomMeal()
    {
        var meal = Database.Meals[UnityEngine.Random.Range(0, Database.Meals.Count)];
        var oven = CookingOvens.FirstOrDefault(oven => oven.IsAvailable());
        if(oven == null)
        {
            Debug.LogWarning("No available Cooking Oven!");
            return;
        }
        oven.Cook(meal);
    }
    public bool AssignSeat(CustomerBehaviour customer, out ChairBehaviour chair)
    {
        chair = Chairs.FirstOrDefault(chair => chair.IsAvailable());
        if (chair == null) return false;
        
        if (!CanReach(chair.placedObject,out GridObject.Dir emptySide)) return false;

        chair.Assign(customer);
        return true;
    }
    public void RequestMeal(CustomerBehaviour customer)
    {
        ServerJobs.Enqueue(new ServeCustomerJob(customer));
    }
    public ServerJob GetServerJob()
    {
        if (ServerJobs.TryDequeue(out var job))
            return job;
        else
            return null;
    }
    public bool GetServingTableForCustomerMeal(out ServingTableBehaviour servingTable)
    {
        servingTable = null;
        foreach (var table in ServingTables)
        {
            if (!CanReach(table.placedObject, out _)) continue;
            if (table.meal != null) servingTable = table;
        }
        return servingTable != null;
    }
    public bool GetServingTableForNewMeal(Meal meal,out ServingTableBehaviour servingTable)
    {
        ServingTableBehaviour emptyTable = null, sameMealTable = null;
        foreach (var table in ServingTables)
        {
            if (!CanReach(table.placedObject, out _)) continue;
            if (table.meal == meal) sameMealTable = table;
            if (table.Empty() && emptyTable == null) emptyTable = table;
        }
        servingTable = sameMealTable ?? emptyTable;
        return servingTable != null;
    }
    public bool CanReach(PlacedObject to, out GridObject.Dir side)
    {
        side = default;
        bool canReach = false;
        foreach (var dir in GridObject.Directions)
        {
            var obj = ConstructionManager.instance.GetPlacedObject(to.GetAdjacentTilePos(dir));
            if (obj == null)
            {
                //TODO: add a* grid search to see if this position is reachable else continue
                canReach = true;
                side = dir;
                break;
            }
        }
        return canReach;
    }
    public bool TryMove(NPCBehaviour npc, PlacedObject to, GridObject.Dir side)
    {
        var result = ConstructionManager.instance.GetPlacedObject(to.GetAdjacentTilePos(side)) == null;
        if (result)
        {
            npc.Move(ConstructionManager.instance.GetBorder(to, side));
        }
        return result;
    }
    public bool TryMove(NPCBehaviour npc, PlacedObject to)
    {
        bool moved = false;
        foreach (var dir in GridObject.Directions)
        {
            if (TryMove(npc,to,dir))
            {
                moved = true;
                break;
            }
        }
        return moved;
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
                var cookingOven = obj.GetComponent<CookingOvenBehaviour>();
                CookingOvens.Add(cookingOven);
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
                var servingTable = obj.GetComponent<ServingTableBehaviour>();
                ServingTables.Add(servingTable);
                break;
            case ObjectType.Wallpaper:
                break;
            case ObjectType.Customer:
                var customer = obj.GetComponent<CustomerBehaviour>();
                Customers.Add(customer);
                break;
            case ObjectType.Chef:
                var chef = obj.GetComponent<ChefBehaviour>();
                Chefs.Add(chef);
                break;
            case ObjectType.Server:
                Servers.Add(obj);
                break;
            default:
                break;
        }

        foreach (var dir in GridObject.Directions)
        {
            UpdateCache(ConstructionManager.instance.GetPlacedObject(obj.GetAdjacentTilePos(dir)));
        }
    }
    private void UpdateCache(PlacedObject obj)
    {
        if (obj == null) return;
        switch (obj.Type)
        {
            case ObjectType.NaN:
                break;
            case ObjectType.Chair:
                UpdateChair(obj, out _);
                break;
            case ObjectType.CookingOven:
                break;
            case ObjectType.DiningTable:
                break;
            case ObjectType.FloorTile:
                break;
            case ObjectType.ServingTable:
                break;
            case ObjectType.Wallpaper:
                break;
            case ObjectType.Door:
                break;
            case ObjectType.Customer:
                break;
            case ObjectType.Chef:
                break;
            case ObjectType.Server:
                break;
            default:
                break;
        }
    }
    private void UpdateChair(PlacedObject obj, out ChairBehaviour chair)
    {
        chair = obj.GetComponent<ChairBehaviour>();
        var frontObj = ConstructionManager.instance.GetPlacedObject(obj.GetFrontTilePos());
        if (frontObj != null && frontObj.Type == ObjectType.DiningTable)
            chair.SetDiningTable(frontObj.GetComponent<DiningTableBehaviour>());
    }

    public void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || GameManager.PointerOverUI) return;

        var go = Mouse3D.GetClickedGameObject();
        if(go != null && go.TryGetComponent<RaycastTarget>(out var raycastTarget))
        {
            raycastTarget.OnClick();
            var gridObject = raycastTarget.GetData();
            Debug.Log(gridObject.ToString());
        }
    }

}
