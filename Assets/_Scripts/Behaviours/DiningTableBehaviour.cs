using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiningTableBehaviour : BaseBehaviour
{
    enum State
    {
        Empty,
        Blocked,
        Dining,
        Trash,
    }
    private State state;
    public CustomerBehaviour customer;
    public override void OnClick()
    {
        Debug.Log("clicked on a dining table!");
    }
    public void Dine(GridObject.Dir dir, CustomerBehaviour customer)
    {
        state = State.Dining;
        this.customer = customer;
    }
    public void FinishDining(GridObject.Dir dir)
    {
        //TODO: Create Clear Table Job and make state = trash
        this.customer = null;
        state = State.Empty;
    }
    public void ClearTable(GridObject.Dir dir)
    {
        state = State.Empty;
    }

    public bool IsAvailable(GridObject.Dir dir)
    {
        return state == State.Empty;
    }

}
