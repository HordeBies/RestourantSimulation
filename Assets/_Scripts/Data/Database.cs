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

    public static int RequiredExp(int level) => 10 * level * (level + 1) / 2;
    
    public Database()
    {
        instance = this;
    }

    [SerializeField] public List<GridObject> floorTiles;
    [SerializeField] public List<GridObject> wallPapers;
    [SerializeField] public List<GridObject> chairs;
    [SerializeField] public List<GridObject> diningTables;
    [SerializeField] public List<GridObject> servingTables;
    [SerializeField] public List<GridObject> cookingOvens;
    [SerializeField] public List<GridObject> doors;

    [SerializeField] public List<GridObject> chefs;
    [SerializeField] public List<GridObject> servers;
    [SerializeField] public List<GridObject> customers;

    [SerializeField] public List<Meal> meals;
}
