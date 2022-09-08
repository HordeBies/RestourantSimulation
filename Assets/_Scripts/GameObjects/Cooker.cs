using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Cooker_", menuName = "GridObject/Cooker")]
public class Cooker : GridObject
{
    public override ObjectType Type => ObjectType.Cooker;
}

