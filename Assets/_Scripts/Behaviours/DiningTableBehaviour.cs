using System.Collections;
using System.Collections.Generic;
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
    private Dictionary<GridObject.Dir, State> positions = new()
    {
        { GridObject.Dir.Down, State.Empty },
        { GridObject.Dir.Left, State.Empty },
        { GridObject.Dir.Right, State.Empty },
        { GridObject.Dir.Up, State.Empty },
    };
    public override void OnClick()
    {
        Debug.Log("clicked on a dining table!");
    }
    public void Dine(GridObject.Dir dir)
    {
        positions[dir] = State.Dining;
    }
    public void FinishDining(GridObject.Dir dir)
    {
        positions[dir] = State.Trash;
    }
    public void ClearTable(GridObject.Dir dir)
    {
        positions[dir] = State.Empty;
    }

    public bool IsAvailable(GridObject.Dir dir)
    {
        return positions[dir] == State.Empty;
    }

}
