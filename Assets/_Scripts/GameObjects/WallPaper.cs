using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WallPaper_", menuName = "GridObject/WallPaper")]
public class WallPaper : GridObject
{
    public override ObjectType Type => ObjectType.Wallpaper;
}

