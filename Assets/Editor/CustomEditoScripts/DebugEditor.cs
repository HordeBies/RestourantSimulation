using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugEditor))]
[InitializeOnLoad]
public class DebugEditor : Editor
{
	[MenuItem("Bies/Clear Cafe")]
	static void ClearCafe()
	{
        if (!Application.isPlaying) return;
        var manager = GameDataManager.instance;
        var constr = ConstructionManager.instance;
        var placed = manager.gameData.CafePlacedObjects;
        var gold = manager.gameData.gold;
        while (placed.Count > 0)
        {
            constr.Demolish(placed[0].Data,placed[0].pos);
        }
        foreach (var item in manager.gameData.ChefData)
        {
            if (item.behaviour == null) continue;
            constr.Demolish(item.behaviour.placedObject, default);
            item.isHired = false;
        }
        foreach (var item in manager.gameData.ServerData)
        {
            if (item.behaviour == null) continue;
            constr.Demolish(item.behaviour.placedObject, default);
            item.isHired = false;
        }
        foreach (var item in manager.gameData.CookBookData)
        {
            item.isLearned = false;
        }
        manager.gameData.gold = gold;
        manager.UpdateFunds();
	}
    [MenuItem("Bies/Reset Progress")]
    static void ResetProgress()
    {
        if (!Application.isPlaying) return;
        var manager = GameDataManager.instance;
        //TODO: Reset progress
    }
    [MenuItem("Bies/Load Default Cafe")]
	static void LoadCafe()
    {
        if (!Application.isPlaying) return;
        GameManager.instance.SwitchToDefaultMode();
        ClearCafe();
        var manager = ConstructionManager.instance;
        var cafe = CafeSimulationManager.instance;
        

        //Walls
        for (int i = 0; i < 11; i++)
        {
            manager.Spawn(i, 0, GridObject.Dir.Up, Database.WallPapers[0]);
            manager.Spawn(0, i, GridObject.Dir.Right, Database.WallPapers[0]);
        }
        //door
        manager.Spawn(1, 2, GridObject.Dir.Left, Database.Doors[0]);
        //Dining Place
        manager.Spawn(3, 5, GridObject.Dir.Down, Database.DiningTables[0]);
        manager.Spawn(3, 6, GridObject.Dir.Down, Database.DiningTables[0]);
        manager.Spawn(2, 5, GridObject.Dir.Right, Database.Chairs[0]);
        manager.Spawn(2, 6, GridObject.Dir.Right, Database.Chairs[0]);
        manager.Spawn(3, 7, GridObject.Dir.Down, Database.DiningTables[0]);
        manager.Spawn(3, 8, GridObject.Dir.Down, Database.DiningTables[0]);
        manager.Spawn(4, 7, GridObject.Dir.Left, Database.Chairs[0]);
        manager.Spawn(4, 8, GridObject.Dir.Left, Database.Chairs[0]);

        //Cooking Place
        manager.Spawn(8, 5, GridObject.Dir.Right, Database.CookingOvens[0]);
        manager.Spawn(8, 4, GridObject.Dir.Right, Database.ServingTables[0]);

        //FloorTiles
        for (int i = 1; i < 11; i++)
        {
            for (int j = 1; j < 11; j++)
            {
                manager.Spawn(i, j, GridObject.Dir.Down, Database.FloorTiles[0]);
            }
        }

        //manager.Spawn(1, 2, GridObject.Dir.Down, Database.Chefs[0]);//Chef at door
        //manager.Spawn(1, 2, GridObject.Dir.Down, Database.Servers[0]);//Server at door
        CafeSimulationManager.instance.GetServingTableForNewMeal(Database.Meals[0], out var table);
        table.PutMeal(Database.Meals[0]);
    }
}
