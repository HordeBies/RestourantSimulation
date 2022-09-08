using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Chair_", menuName = "GridObject/Chair")]
public class Chair : GridObject
{
    public override ObjectType Type => ObjectType.Chair;
}

