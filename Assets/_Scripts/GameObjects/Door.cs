using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Door_", menuName = "GridObject/Door")]
public class Door : GridObject
{
    public override ObjectType Type => ObjectType.Door;
}
