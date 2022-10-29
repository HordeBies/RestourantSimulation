using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Database DataBase;

    public int gold;
    public int gems;

    public int level;
    public int exp;
    
    public string username;

    public GameData()
    {
        gold = 100;
        gems = 10;
        level = 1;
        exp = 0;
        username = "Guest_3169";
        Storage = new();
        CafePlacedObjects = new();
        CookBookData = new();
        ChefData = new();
        ServerData = new();
    }
    public void Populate()
    {
        foreach (var item in DataBase.diningTables)
            Storage.diningTables.Add((item));
        foreach (var item in DataBase.chairs)
            Storage.chairs.Add((item));
        foreach (var item in DataBase.cookingOvens)
            Storage.cookingOvens.Add((item));
        foreach (var item in DataBase.doors)
            Storage.doors.Add((item));
        foreach (var item in DataBase.floorTiles)
            Storage.floorTiles.Add((item));
        foreach (var item in DataBase.servingTables)
            Storage.servingTables.Add((item));
        foreach (var item in DataBase.wallPapers)
            Storage.wallPapers.Add((item));

        foreach (var item in DataBase.chefs)
            ChefData.Add(new(item));
        foreach (var item in DataBase.servers)
            ServerData.Add(new(item));
    }
    public MainStorage Storage;
    [System.Serializable]
    public class MainStorage
    {
        public List<GridObject> diningTables = new();
        public List<GridObject> chairs = new();
        public List<GridObject> cookingOvens = new();
        public List<GridObject> doors = new();
        public List<GridObject> floorTiles = new();
        public List<GridObject> servingTables = new();
        public List<GridObject> wallPapers = new();
        public List<GridObject> GetList(ObjectType type)
        {
            switch (type)
            {
                case ObjectType.Chair:
                    return chairs;
                case ObjectType.CookingOven:
                    return cookingOvens;
                case ObjectType.DiningTable:
                    return diningTables;
                case ObjectType.FloorTile:
                    return floorTiles;
                case ObjectType.ServingTable:
                    return servingTables;
                case ObjectType.Wallpaper:
                    return wallPapers;
                case ObjectType.Door:
                    return doors;
                default:
                    throw new System.Exception("Not a valid storage item type!");
            }
        }
        public GridObject Find(GridObject obj)
        {
            return GetList(obj.Type).Find(i => i == obj);
        }
    }
    public List<CafeData> CafePlacedObjects;
    [System.Serializable]
    public class CafeData
    {
        public PlacedObject Data;
        public Vector2Int pos;
        public GridObject.Dir dir;
        public BaseBehaviour behaviour;
    }
    public List<MealData> CookBookData;
    [System.Serializable]
    public class MealData
    {
        public Meal meal;
        public bool isLearned;
    }
    public List<WorkerData> ChefData;
    public List<WorkerData> ServerData;
    [System.Serializable]
    public class WorkerData
    {
        public WorkerData(GridObject worker)
        {
            this.worker = worker;
            isHired = false;
        }
        public GridObject worker;
        public bool isHired;
    }
}
