using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionPanel : MonoBehaviour
{
    [SerializeField] private UISlot ConstructionUISlotPF;
    [Space]
    [SerializeField] private Transform Content;
    [SerializeField] private int SlotCount;
    private UISlot[] uiSlots;

    private Action<UISlot> onClick;

    private void Awake()
    {
        ConstructionManager.OnSelectedChanged += Refresh;
        onClick = (slot) =>
        {
            ConstructionManager.instance.SetSelectedGridObject(slot.gridObject);
        };
        Initialize();
    }
    private UISlot CreateSlot()
    {
        GameObject createdUISlot = Instantiate(ConstructionUISlotPF.gameObject, Content);
        return createdUISlot.GetComponent<UISlot>();
    }
    private void Initialize()
    {
        CreateSlot().Populate(null as GridObject, (slot) => ConstructionManager.instance.ResetSelectedGridObjectSO());

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
        uiSlots = Content.GetComponentsInChildren<UISlot>();
        Refresh(null);
    }
    public void Refresh(GridObject selectedGO)
    {
        var selected = uiSlots.FirstOrDefault(slot => slot.gridObject == selectedGO);
        if (selected == null) selected = uiSlots[0];
        foreach (var slot in uiSlots)
        {
            slot.Refresh(selected);
        }
    }
}
