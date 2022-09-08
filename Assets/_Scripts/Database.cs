using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class Database : ScriptableObject
{
    public static Database instance;
    public static List<FloorTile> FloorTiles => instance.floorTiles;
    public static List<WallPaper> WallPapers => instance.wallPapers;
    public static List<Chair> Chairs => instance.chairs;
    public static List<DiningTable> DiningTables => instance.diningTables;
    public static List<Door> Doors => instance.doors;
    public static List<CookingOven> CookingOvens => instance.cookingOvens;
    public static List<ServingTable> ServingTables => instance.servingTables;


    public static UISlot UISlotPrefab => instance.uiSlotPrefab;
    public Database()
    {
        instance = this;
    }

    [SerializeField] private List<FloorTile> floorTiles;
    [SerializeField] private List<WallPaper> wallPapers;
    [SerializeField] private List<Chair> chairs;
    [SerializeField] private List<DiningTable> diningTables;
    [SerializeField] private List<ServingTable> servingTables;
    [SerializeField] private List<CookingOven> cookingOvens;
    [SerializeField] private List<Door> doors;

    [SerializeField] private List<Cooker> chefs;
    [SerializeField] private List<Server> servers;
    [SerializeField] private List<Customer> Customers;

    [SerializeField] private UISlot uiSlotPrefab;
}
