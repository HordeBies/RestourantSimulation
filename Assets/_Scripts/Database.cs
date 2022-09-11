using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class Database : ScriptableObject
{
    public static Database instance;
    public static List<GridObject> FloorTiles => instance.floorTiles;
    public static List<GridObject> WallPapers => instance.wallPapers;
    public static List<GridObject> Chairs => instance.chairs;
    public static List<GridObject> DiningTables => instance.diningTables;
    public static List<GridObject> Doors => instance.doors;
    public static List<GridObject> CookingOvens => instance.cookingOvens;
    public static List<GridObject> ServingTables => instance.servingTables;

    public static List<GridObject> Chefs => instance.chefs;
    public static List<GridObject> Servers => instance.servers;
    public static List<GridObject> Customers => instance.customers;
    
    public static List<Meal> Meals => instance.meals;
    
    public Database()
    {
        instance = this;
    }

    [SerializeField] private List<GridObject> floorTiles;
    [SerializeField] private List<GridObject> wallPapers;
    [SerializeField] private List<GridObject> chairs;
    [SerializeField] private List<GridObject> diningTables;
    [SerializeField] private List<GridObject> servingTables;
    [SerializeField] private List<GridObject> cookingOvens;
    [SerializeField] private List<GridObject> doors;

    [SerializeField] private List<GridObject> chefs;
    [SerializeField] private List<GridObject> servers;
    [SerializeField] private List<GridObject> customers;

    [SerializeField] private List<Meal> meals;
}
