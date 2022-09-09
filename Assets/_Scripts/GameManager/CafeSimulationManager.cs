using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CafeSimulationManager : MonoBehaviour
{
    public static CafeSimulationManager instance;
    public static Func<PlacedObject> GetDoorTile;

    private List<PlacedObject> DiningTables = new();
    private List<PlacedObject> ServingTables = new();


    private List<PlacedObject> Customers = new();
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

    private void AddNewObject(PlacedObject obj)
    {
        switch (obj.Type)
        {
            case ObjectType.NaN:
                Debug.LogError("NAN Object Type Spawned!");
                break;
            case ObjectType.Chair:
                break;
            case ObjectType.CookingOven:
                break;
            case ObjectType.DiningTable:
                DiningTables.Add(obj);
                break;
            case ObjectType.FloorTile:
                break;
            case ObjectType.ServingTable:
                ServingTables.Add(obj);
                break;
            case ObjectType.Wallpaper:
                break;
            case ObjectType.Customer:
                Customers.Add(obj);
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

    public void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || GameManager.PointerOverUI) return;

        Debug.Log("Left Click on Default outside UI");
    }

}
