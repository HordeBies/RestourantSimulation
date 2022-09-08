using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CookingOven_", menuName = "GridObject/CookingOven")]
public class CookingOven : GridObject
{
    public override ObjectType Type => ObjectType.CookingOven;
}

