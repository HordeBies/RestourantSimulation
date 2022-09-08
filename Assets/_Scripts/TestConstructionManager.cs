using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConstructionManager : MonoBehaviour
{
    ConstructionManager manager;
    private void Start()
    {
        manager = ConstructionManager.instance;

        //Walls
        for (int i = 0; i < 11; i++)
        {
            manager.TestBuild(i, 0, GridObject.Dir.Up, Database.WallPapers[0]);
            manager.TestBuild(0, i, GridObject.Dir.Right, Database.WallPapers[0]);
        }
        //door
        manager.TestBuild(1, 2, GridObject.Dir.Left, Database.Doors[0]);
        //Dining Place
        manager.TestBuild(3, 5, GridObject.Dir.Down, Database.DiningTables[0]);
        manager.TestBuild(3, 6, GridObject.Dir.Down, Database.DiningTables[0]);
        manager.TestBuild(2, 5, GridObject.Dir.Right, Database.Chairs[0]);
        manager.TestBuild(2, 6, GridObject.Dir.Right, Database.Chairs[0]);
        manager.TestBuild(4, 5, GridObject.Dir.Left, Database.Chairs[0]);
        manager.TestBuild(4, 6, GridObject.Dir.Left, Database.Chairs[0]);

        //Cooking Place
        manager.TestBuild(8, 5, GridObject.Dir.Right, Database.CookingOvens[0]);
        manager.TestBuild(8, 4, GridObject.Dir.Right, Database.ServingTables[0]);

        //FloorTiles
        for (int i = 1; i < 11; i++)
        {
            for (int j = 1; j < 11; j++)
            {
                manager.TestBuild(i, j, GridObject.Dir.Down, Database.FloorTiles[0]);
            }
        }

    }

}
