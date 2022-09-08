using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Bies
{
    public class GridBuildingSystem : MonoBehaviour
    {
        public static GridBuildingSystem Instance { get; private set; }



        private void Awake()
        {
            Instance = this;

            
        }

        private void Update()
        {
            //if (Input.GetMouseButtonDown(0) && placedGridObjectSO != null)
            //{
            //    grid.GetXZ(Mouse3D.GetMouseWorldPosition(), out int x, out int z);

            //    var gridPositionList = placedGridObjectSO.GetGridPositionList(new Vector2Int(x, z), dir);
            //    var gridObject = grid.GetGridObject(x, z);
            //    if (gridPositionList.All(position => grid.GetGridObject(position.x, position.y)?.CanBuild ?? false))
            //    {
            //        var rotationOffset = placedGridObjectSO.GetRotationOffset(dir);
            //        var placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x,0,rotationOffset.y) * grid.GetCellSize();
            //        var placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, placedGridObjectSO);
            //        gridPositionList.ForEach(position => grid.GetGridObject(position.x, position.y).SetPlacedObject(placedObject));

            //        OnObjectPlaced?.Invoke(this, EventArgs.Empty);
            //    }
            //    else
            //        UtilsClass.CreateWorldTextPopup("Cannot build here!", Mouse3D.GetMouseWorldPosition());
            //}

            //if (Input.GetMouseButtonDown(1))
            //{
            //    var gridObject = grid.GetGridObject(Mouse3D.GetMouseWorldPosition());
            //    var placedObject = gridObject?.GetPlacedObject();
            //    if (placedObject != null)
            //    {
            //        var gridPositionList = placedObject.GetGridPositionList();
            //        gridPositionList.ForEach(position => grid.GetGridObject(position.x, position.y).ClearPlacedObject());
            //        placedObject.DestroySelf();
            //    }
            //}

            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    dir = GridObject.GetNextDir(dir);
            //    UtilsClass.CreateWorldTextPopup(dir.ToString(), Mouse3D.GetMouseWorldPosition());
            //}
            
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    if (placedGridObjectSO == null) placedGridObjectSOidx = 0;
            //    else placedGridObjectSOidx = (placedGridObjectSOidx - 1 + placedGridObjectSOList.Count) % placedGridObjectSOList.Count;

            //    placedGridObjectSO = placedGridObjectSOList[placedGridObjectSOidx];
            //    UtilsClass.CreateWorldTextPopup(placedGridObjectSO.name, Mouse3D.GetMouseWorldPosition());
            //    OnSelectedChanged?.Invoke(this, EventArgs.Empty);
            //}
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    if (placedGridObjectSO == null) placedGridObjectSOidx = 0;
            //    else placedGridObjectSOidx = (placedGridObjectSOidx + 1) % placedGridObjectSOList.Count;
            //    placedGridObjectSO = placedGridObjectSOList[placedGridObjectSOidx];
            //    UtilsClass.CreateWorldTextPopup(placedGridObjectSO.name, Mouse3D.GetMouseWorldPosition());
            //    OnSelectedChanged?.Invoke(this, EventArgs.Empty);
            //}
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    placedGridObjectSO = null;
            //    UtilsClass.CreateWorldTextPopup("exit build mode", Mouse3D.GetMouseWorldPosition());
            //    OnSelectedChanged?.Invoke(this, EventArgs.Empty);
            //}
        }
        

    }

}