using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssignChefHUD : HUD
{
    [SerializeField] private Transform container;
    private List<UISlot> chefSlots = new();
    public ChefBehaviour selectedChef { private set; get; }

    private Action<UISlot> onClick;
    private void Awake()
    {
        onClick = (slot) =>
        {
            selectedChef = slot.chef;
            Refresh();
        };

        var slot = CreateSlot(container);
        slot.Populate(null as ChefBehaviour, onClick);
        chefSlots.Add(slot);
        foreach (var chef in cafe.Chefs)
        {
            slot = CreateSlot(container);
            slot.Populate(chef, onClick);
            chefSlots.Add(slot);
        }
    }
    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public override void Open(BaseBehaviour data)
    {
        gameObject.SetActive(true);
        selectedChef = (ChefBehaviour)data;
        Refresh();
    }

    public override void Refresh()
    {
        var selectedUISlot = chefSlots.FirstOrDefault(slot => slot.chef == selectedChef);
        if (selectedUISlot == null) selectedUISlot = chefSlots[0];
        foreach (var chefSlot in chefSlots)
        {
            chefSlot.Refresh(selectedUISlot);
        }
    }
}
