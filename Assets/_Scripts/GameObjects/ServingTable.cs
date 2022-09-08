using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ServingTable_", menuName = "GridObject/ServingTable")]
public class ServingTable : GridObject
{
    public override ObjectType Type => ObjectType.ServingTable;
}

