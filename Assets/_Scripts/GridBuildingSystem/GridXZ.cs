using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridXZ<TGridObject>
{

    public event EventHandler<OnGridTileChangedEventArgs> OnGridTileChanged;
    public class OnGridTileChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    private int width;
    private int length;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    public GridXZ(int width, int length, float cellSize, Vector3 originPosition, Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.length = length;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, length];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = createGridObject(this, x, z);
            }
        }

        bool showDebug = GameManager.instance.showGridDebug;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, length];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    debugTextArray[x, z] = UtilsClass.CreateWorldText(gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 15, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white,1000f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 1000f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, length), GetWorldPosition(width, length), Color.white, 1000f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, length), Color.white, 1000f);

            OnGridTileChanged += (object sender, OnGridTileChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetLength()
    {
        return length;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public void SetGridTile(int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < width && z < length)
        {
            gridArray[x, z] = value;
            TriggerGridTileChanged(x, z);
        }
    }

    public void TriggerGridTileChanged(int x, int z)
    {
        OnGridTileChanged?.Invoke(this, new OnGridTileChangedEventArgs { x = x, z = z });
    }

    public void SetGridTile(Vector3 worldPosition, TGridObject value)
    {
        GetXZ(worldPosition, out int x, out int z);
        SetGridTile(x, z, value);
    }

    public TGridObject GetGridTile(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < length)
        {
            return gridArray[x, z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridTile(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridTile(x, z);
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        return new Vector2Int(
            Mathf.Clamp(gridPosition.x, 0, width - 1),
            Mathf.Clamp(gridPosition.y, 0, length - 1)
        );
    }

}
