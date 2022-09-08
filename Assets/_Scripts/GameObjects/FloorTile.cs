using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorTile_", menuName = "GridObject/FloorTile")]
public class FloorTile : GridObject
{
    public override ObjectType Type => ObjectType.FloorTile;
}
