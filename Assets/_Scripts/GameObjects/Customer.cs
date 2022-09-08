using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Customer_", menuName = "GridObject/Customer")]
public class Customer : GridObject
{
    public override ObjectType Type => ObjectType.Customer;
}

