using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionPanel : MonoBehaviour
{
    [SerializeField] private Transform Content;
    [SerializeField] private int SlotCount;

    private Action<UISlot> onClick;

    private void Awake()
    {
        onClick = (slot) =>
        {
            ConstructionManager.instance.SetSelectedGridObject(slot);
        };
        Initialize();
    }
    private void OnEnable()
    {
        Refresh();
    }
    private UISlot CreateSlot()
    {
        GameObject createdUISlot = Instantiate(Database.UISlotPrefab.gameObject, Content);
        return createdUISlot.GetComponent<UISlot>();
    }
    private void Initialize()
    {
        CreateSlot().Populate(null, (slot) => ConstructionManager.instance.ResetSelectedGridObjectSO());

        foreach (var item in Database.FloorTiles)
        {
            CreateSlot().Populate(item,onClick);
        }

        foreach (var item in Database.WallPapers)
        {
            CreateSlot().Populate(item, onClick);
        }

        foreach (var item in Database.Chairs)
        {
            CreateSlot().Populate(item, onClick);
        }

        foreach (var item in Database.DiningTables)
        {
            CreateSlot().Populate(item, onClick);
        }

        foreach (var item in Database.Doors)
        {
            CreateSlot().Populate(item, onClick);
        }
        
        foreach (var item in Database.CookingOvens)
        {
            CreateSlot().Populate(item, onClick);
        }
          
        foreach (var item in Database.ServingTables)
        {
            CreateSlot().Populate(item, onClick);
        }
    }
    public void Refresh()
    {

    }
}
