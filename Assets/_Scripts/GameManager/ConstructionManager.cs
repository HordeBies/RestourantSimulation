using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager instance;

    public static Action<GridObject> OnSelectedChanged;
    public static event EventHandler OnObjectPlaced;

    private GridXZ<CafeGridTile> CafeGrid;

    private GridObject selectedGridObject;

    private GridObject.Dir dir = GridObject.Dir.Down;

    private void Awake()
    {
        instance = this;

        int gridWidth = 11;
        int gridLength = 11;
        float cellSize = 10f;
        Vector3 ori = Vector3.zero;
        CafeGrid = new(gridWidth, gridLength, cellSize, ori - Vector3.forward * cellSize - Vector3.right * cellSize, (g, x, z) => { return new(g, x, z); });

        selectedGridObject = null;
    }
    public void SetSelectedGridObject(UISlot from)
    {
        selectedGridObject = from?.gridObject;
        Debug.Log("Clicked to " + selectedGridObject.nameString);
        OnSelectedChanged?.Invoke(selectedGridObject);
    }
    public GridObject GetSelectedGridObjectSO()
    {
        return selectedGridObject;
    }

    public void ResetSelectedGridObjectSO()
    {
        selectedGridObject = null;
        OnSelectedChanged?.Invoke(selectedGridObject);
    }

    #region HelperMethods
    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        CafeGrid.GetXZ(mousePosition, out int x, out int z);

        if (selectedGridObject != null)
        {
            Vector2Int rotationOffset = selectedGridObject.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = CafeGrid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * CafeGrid.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Quaternion GetSelectedGridObjectRotation()
    {
        if (selectedGridObject != null)
        {
            return Quaternion.Euler(0, selectedGridObject.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }
    public void TestBuild(int x, int z,GridObject.Dir newDir, GridObject newSelectedGridObject)
    {
        dir = newDir;
        selectedGridObject = newSelectedGridObject;
        Build(x, z);
    }
    public void Build(int x, int z)
    {
        var gridPositionList = selectedGridObject.GetGridPositionList(new Vector2Int(x, z), dir);
        var gridObject = CafeGrid.GetGridObject(x, z);
        if (gridPositionList.All(position => CafeGrid.GetGridObject(position.x, position.y)?.CanBuild(selectedGridObject) ?? false))
        {
            var rotationOffset = selectedGridObject.GetRotationOffset(dir);
            var placedObjectWorldPosition = CafeGrid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * CafeGrid.GetCellSize();
            var placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, selectedGridObject);
            gridPositionList.ForEach(position => CafeGrid.GetGridObject(position.x, position.y).SetPlacedObject(placedObject));

            OnObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
        else
            UtilsClass.CreateWorldTextPopup("Cannot build here!", Mouse3D.GetMouseWorldPosition());
    }
    #endregion
    
    #region ConstructionInputHandling
    public void OnLeftClick(InputAction.CallbackContext ctx) //Place Grid Object
    {
        if (!ctx.started || GameManager.PointerOverUI || selectedGridObject == null) return;

        Debug.Log("Left Click from Construction mode outside UI");
        CafeGrid.GetXZ(Mouse3D.GetMouseWorldPosition(), out int x, out int z);

        var gridPositionList = selectedGridObject.GetGridPositionList(new Vector2Int(x, z), dir);
        var gridObject = CafeGrid.GetGridObject(x, z);
        if (gridPositionList.All(position => CafeGrid.GetGridObject(position.x, position.y)?.CanBuild(selectedGridObject) ?? false))
        {
            var rotationOffset = selectedGridObject.GetRotationOffset(dir);
            var placedObjectWorldPosition = CafeGrid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * CafeGrid.GetCellSize();
            var placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, selectedGridObject);
            gridPositionList.ForEach(position => CafeGrid.GetGridObject(position.x, position.y).SetPlacedObject(placedObject));

            OnObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
        else
            UtilsClass.CreateWorldTextPopup("Cannot build here!", Mouse3D.GetMouseWorldPosition());
    }

    public void OnRightClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;

        if(selectedGridObject != null)
        {
            ResetSelectedGridObjectSO();
            return;
        }
        var gridObject = CafeGrid.GetGridObject(Mouse3D.GetMouseWorldPosition());
        var placedObject = gridObject?.GetPlacedObject();
        if (placedObject != null)
        {
            var gridPositionList = placedObject.GetGridPositionList();
            gridPositionList.ForEach(position => CafeGrid.GetGridObject(position.x, position.y).ClearPlacedObject());
            placedObject.DestroySelf();
        }
    }

    public void OnRotateClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        dir = GridObject.GetNextDir(dir);
        UtilsClass.CreateWorldTextPopup(dir.ToString(), Mouse3D.GetMouseWorldPosition());
    }

    #endregion
}
public class CafeGridTile
{
    protected GridXZ<CafeGridTile> grid;
    protected int x;
    protected int z;

    private PlacedObject placedObject;
    private PlacedObject floorTile;
    private static PlacedObject doorTile;

    public bool CanBuild(GridObject gridObject)
    {
        bool res = false;
        switch (gridObject.Type)
        {
            case ObjectType.FloorTile:
                if(x > 0 && z > 0) res = true;
                break;
            case ObjectType.Door:
                if ((((x == 1) != (z == 1))) && (x > 0 && z > 0)) res = true;
                break;
            case ObjectType.Wallpaper:
                if ((x == 0) != (z == 0)) res = true; //simple XOR to prevent (0,0)
                break;
            default:
                res = x > 0 && z > 0 && placedObject == null;
                break;
        }
        return res;
    }

    public CafeGridTile(GridXZ<CafeGridTile> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }

    public void SetPlacedObject(PlacedObject newPlacedObject)
    {
        switch (newPlacedObject?.Type)
        {
            case ObjectType.FloorTile:
                //TODO: Pack up old floor tile
                floorTile?.DestroySelf();
                floorTile = newPlacedObject;
                break;
            case ObjectType.Door:
                doorTile?.DestroySelf();
                doorTile = newPlacedObject;
                break;
            case ObjectType.Wallpaper:
                //TODO: Pack up old wall tile
                placedObject?.DestroySelf();
                placedObject = newPlacedObject;
                break;
            default:
                placedObject = newPlacedObject;
                break;
        }

        grid.TriggerGridObjectChanged(x, z);
    }

    public PlacedObject GetPlacedObject()
    {
        return placedObject ?? floorTile;
    }
    public PlacedObject GetPlacedFloorTile()
    {
        return floorTile;
    }
    public void ClearPlacedObject()
    {
        if (placedObject != null) placedObject = null;
        else if (floorTile != null) floorTile = null;
        grid.TriggerGridObjectChanged(x, z);
    }
    public void ClearFloorTile()
    {
        floorTile = null;
        grid.TriggerGridObjectChanged(x, z);
    }

    public override string ToString()
    {
        return x + " , " + z + "\n" + placedObject;
    }
}

