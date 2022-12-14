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
    public static Action<PlacedObject> OnObjectPlaced;
    public static Action<PlacedObject> OnObjectRemoved;

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
    public void SetSelectedGridObject(GridObject to)
    {
        selectedGridObject = to;
        Debug.Log("Clicked to " + selectedGridObject?.nameString);
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

    public PlacedObject GetPlacedObject(Vector2Int coord)
    {
        return CafeGrid.GetGridTile(coord.x, coord.y)?.GetPlacedObject();
    }
    public Vector3 GetBorder(PlacedObject obj, GridObject.Dir dir)
    {
        var pos = CafeGrid.GetWorldPosition(obj.origin.x,obj.origin.y);
        //TODO: take into consideration of bigger than 1x1 objects
        switch (dir)
        {
            default:
            case GridObject.Dir.Down: return new(pos.x + CafeGrid.GetCellSize() / 2, pos.y, pos.z);
            case GridObject.Dir.Left: return new(pos.x, pos.y, pos.z + CafeGrid.GetCellSize() / 2);
            case GridObject.Dir.Up: return new(pos.x + CafeGrid.GetCellSize() / 2, pos.y, pos.z + CafeGrid.GetCellSize());
            case GridObject.Dir.Right: return new(pos.x + CafeGrid.GetCellSize(), pos.y, pos.z + CafeGrid.GetCellSize() / 2);
        }
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
    public void SpawnAtDoor(GridObject gridObjectToSpawn)
    {
        Spawn(CafeGridTile.DoorTile.origin, GridObject.GetReverseDir(CafeGridTile.DoorTile.dir), gridObjectToSpawn);
    }
    public void Spawn(Vector2Int pos, GridObject.Dir newDir, GridObject newSelectedGridObject)
    {
        int x = pos.x;
        int z = pos.y;
        Spawn(x, z, newDir, newSelectedGridObject);
    }

    public void Spawn(int x, int z,GridObject.Dir newDir, GridObject newSelectedGridObject)
    {
        dir = newDir;
        selectedGridObject = newSelectedGridObject;
        Build(x, z);
        selectedGridObject = null;
    }
    public void Build(int x, int z)
    {
        var gridPositionList = selectedGridObject.GetGridPositionList(new Vector2Int(x, z), dir);
        if (gridPositionList.All(position => CafeGrid.GetGridTile(position.x, position.y)?.CanBuild(selectedGridObject) ?? false))
        {
            var rotationOffset = selectedGridObject.GetRotationOffset(dir);
            var placedObjectWorldPosition = CafeGrid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * CafeGrid.GetCellSize();
            var placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, selectedGridObject);
            foreach (var position in gridPositionList)
            {
                CafeGrid.GetGridTile(position.x, position.y).SetPlacedObject(placedObject, out var previous);
                if (previous != null) OnObjectRemoved?.Invoke(previous);
            }
            OnObjectPlaced?.Invoke(placedObject);
        }
        else
            UtilsClass.CreateWorldTextPopup("Cannot build here!", Mouse3D.GetMouseWorldPosition());
    }
    public void Demolish(PlacedObject obj, Vector2Int pos)
    {
        var gridObject = CafeGrid.GetGridTile(pos);
        if (gridObject.RemovePlacedObject(obj))
        {
            OnObjectRemoved?.Invoke(obj);
            obj.DestroySelf();
        }
        else
        {
            Debug.LogError("Couldn't demolish");
        }
    }
    #endregion
    
    #region ConstructionInputHandling
    public void OnLeftClick(InputAction.CallbackContext ctx) //Place Grid Object
    {
        if (GameManager.PointerOverUI) return;
        if (!ctx.started) return;
        Debug.Log("Left Click from Construction mode outside UI");

        CafeGrid.GetXZ(Mouse3D.GetMouseWorldPosition(), out int x, out int z);
        
        if(selectedGridObject != null)
        {
            Build(x, z);
            return;
        }

        var go = Mouse3D.GetClickedGameObject();
        if (go != null && go.TryGetComponent<RaycastTarget>(out var rt))
        {
            SetSelectedGridObject(rt.placedObject.placedObjectTypeSO);
            dir = rt.placedObject.dir;
            return;
        }

    }

    public void OnRightClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;

        if(selectedGridObject != null)
        {
            ResetSelectedGridObjectSO();
            return;
        }
        var gridObject = CafeGrid.GetGridTile(Mouse3D.GetMouseWorldPosition());
        var placedObject = gridObject?.GetPlacedObject();
        if (placedObject != null)
        {
            var gridPositionList = placedObject.GetGridPositionList();
            gridPositionList.ForEach(position => CafeGrid.GetGridTile(position.x, position.y).ClearPlacedObject());
            OnObjectRemoved?.Invoke(placedObject);
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
    public static PlacedObject DoorTile => doorTile;

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
            case ObjectType.Chef:
            case ObjectType.Server:
            case ObjectType.Customer:
                res = true;
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

    public void SetPlacedObject(PlacedObject newPlacedObject, out PlacedObject previous)
    {
        previous = default;
        switch (newPlacedObject?.Type)
        {
            case ObjectType.FloorTile:
                //TODO: Pack up old floor tile
                previous = floorTile;
                floorTile?.DestroySelf();
                floorTile = newPlacedObject;
                break;
            case ObjectType.Door:
                previous = doorTile;
                doorTile?.DestroySelf();
                doorTile = newPlacedObject;
                break;
            case ObjectType.Wallpaper:
                //TODO: Pack up old wall tile
                previous = floorTile;
                placedObject?.DestroySelf();
                placedObject = newPlacedObject;
                break;
            case ObjectType.Chef:
            case ObjectType.Server:
            case ObjectType.Customer:
                //doesnt place on a tile just let it be in the world
                break;
            default:
                placedObject = newPlacedObject;
                break;
        }
        grid.TriggerGridTileChanged(x, z);
    }

    public PlacedObject GetPlacedObject()
    {
        return placedObject;
    }
    public PlacedObject GetPlacedFloorTile()
    {
        return floorTile;
    }
    public void ClearPlacedObject()
    {
        if (placedObject != null) placedObject = null;
        else if (floorTile != null) floorTile = null;
        grid.TriggerGridTileChanged(x, z);
    }
    public void ClearFloorTile()
    {
        floorTile = null;
        grid.TriggerGridTileChanged(x, z);
    }
    public bool RemovePlacedObject(PlacedObject obj)
    {
        var removed = false;
        if (obj.IsNPC)
        {
            removed = true;
        }
        else if(obj == doorTile)
        {
            doorTile = null;
            removed = true;
        }
        else if(obj == floorTile)
        {
            floorTile = null;
            removed = true;
        }
        else if(obj == placedObject)
        {
            placedObject = null;
            removed = true;
        }
        return removed;
    }

    public override string ToString()
    {
        return x + " , " + z + "\n" + placedObject;
    }
}

