using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairBehaviour : BaseBehaviour
{
    [SerializeField]private CustomerBehaviour customerAssigned;
    [SerializeField]private DiningTableBehaviour diningTable;
    private GridObject.Dir tableDir => GridObject.GetReverseDir(placedObject.dir);
    public override void OnClick()
    {
        Debug.Log("clicked on a chair!");
    }
    public void SetDiningTable(DiningTableBehaviour diningTable)
    {
        this.diningTable = diningTable;
    }
    public bool IsAvailable()
    {
        return customerAssigned == null && diningTable != null && diningTable.IsAvailable(tableDir);
    }
    public void Assign(CustomerBehaviour customer)
    {
        customerAssigned = customer;
        diningTable.Dine(tableDir);
    }
    public void Leave()
    {
        customerAssigned = null;
        diningTable.FinishDining(tableDir);
    }
}
