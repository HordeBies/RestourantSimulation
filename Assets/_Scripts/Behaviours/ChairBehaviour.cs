using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairBehaviour : BaseBehaviour
{
    [SerializeField]private CustomerBehaviour customerAssigned;
    [SerializeField]private DiningTableBehaviour diningTable;
    public Transform customerPlace;
    public GridObject.Dir TableDir => GridObject.GetReverseDir(placedObject.dir);
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
        return customerAssigned == null && diningTable != null && diningTable.IsAvailable(TableDir);
    }
    public void Assign(CustomerBehaviour customer)
    {
        customerAssigned = customer;
        diningTable.Dine(TableDir);
    }
    public void Leave()
    {
        customerAssigned = null;
        diningTable.FinishDining(TableDir);
    }
}
