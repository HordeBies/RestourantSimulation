using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DiningTable_", menuName = "GridObject/DiningTable")]
public class DiningTable : GridObject
{
    public override ObjectType Type => ObjectType.DiningTable;
}

