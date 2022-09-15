using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    NaN,
    Chair,
    CookingOven,
    DiningTable,
    FloorTile,
    ServingTable,
    Wallpaper,
    Door,

    Customer,
    Chef,
    Server,
}
[CreateAssetMenu(fileName = "GridObject_", menuName = "Bies/GridObject")]
public class GridObject : ScriptableObject
{
    public enum Dir
    {
        Down,
        Left,
        Up,
        Right,
    }
    public static Dir[] Directions = new Dir[4] { Dir.Down, Dir.Left, Dir.Up, Dir.Right };
    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return Dir.Left;
            case Dir.Left: return Dir.Up;
            case Dir.Up: return Dir.Right;
            case Dir.Right: return Dir.Down;
        }
    }
    public static Dir GetReverseDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return Dir.Up;
            case Dir.Left: return Dir.Right;
            case Dir.Up: return Dir.Down;
            case Dir.Right: return Dir.Left;
        }
    }
    public ObjectType Type;
    public string nameString;
    public Sprite icon;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int length;
    public bool canBeSelected = true;

    public override string ToString()
    {
        return $"Type = {Type}\nName = {nameString}";
    }
    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 90;
            case Dir.Up: return 180;
            case Dir.Right: return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return new Vector2Int(0, 0);
            case Dir.Left: return new Vector2Int(0, width);
            case Dir.Up: return new Vector2Int(width, length);
            case Dir.Right: return new Vector2Int(length, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < length; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < length; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    }
}
