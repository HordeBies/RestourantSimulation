using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Server_", menuName = "GridObject/Server")]
public class Server : GridObject
{
    public override ObjectType Type => ObjectType.Server;
}

