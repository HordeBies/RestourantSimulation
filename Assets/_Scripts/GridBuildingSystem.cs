using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Bies
{
    public class GridBuildingSystem : MonoBehaviour
    {
        public static GridBuildingSystem Instance { get; private set; }

        public event EventHandler OnSelectedChanged;
        public event EventHandler OnObjectPlaced;

        [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList;
        private PlacedObjectTypeSO placedObjectTypeSO;
        private int placedObjectTypeSOidx;
        private GridXZ<GridObject> grid;
        private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;

        private void Awake()
        {
            Instance = this;

            int gridWidth = 5;
            int gridLength = 5;
            float cellSize = 10f;
            grid = new(gridWidth, gridLength, cellSize, Vector3.zero, (g, x, z) => { return new(g, x, z); });

            placedObjectTypeSOidx = 0;
            placedObjectTypeSO = null;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null)
            {
                grid.GetXZ(Mouse3D.GetMouseWorldPosition(), out int x, out int z);

                var gridPositionList = placedObjectTypeSO.GetGridPositionList(new Vector2Int(x, z), dir);
                var gridObject = grid.GetGridObject(x, z);
                if (gridPositionList.All(position => grid.GetGridObject(position.x, position.y)?.CanBuild ?? false))
                {
                    var rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                    var placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x,0,rotationOffset.y) * grid.GetCellSize();
                    var placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, placedObjectTypeSO);
                    gridPositionList.ForEach(position => grid.GetGridObject(position.x, position.y).SetPlacedObject(placedObject));

                    OnObjectPlaced?.Invoke(this, EventArgs.Empty);
                }
                else
                    UtilsClass.CreateWorldTextPopup("Cannot build here!", Mouse3D.GetMouseWorldPosition());
            }

            if (Input.GetMouseButtonDown(1))
            {
                var gridObject = grid.GetGridObject(Mouse3D.GetMouseWorldPosition());
                var placedObject = gridObject.GetPlacedObject();
                if (placedObject != null)
                {
                    var gridPositionList = placedObject.GetGridPositionList();
                    gridPositionList.ForEach(position => grid.GetGridObject(position.x, position.y).ClearPlacedObject());
                    placedObject.DestroySelf();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                dir = PlacedObjectTypeSO.GetNextDir(dir);
                UtilsClass.CreateWorldTextPopup(dir.ToString(), Mouse3D.GetMouseWorldPosition());
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (placedObjectTypeSO == null) placedObjectTypeSOidx = 0;
                else placedObjectTypeSOidx = (placedObjectTypeSOidx - 1 + placedObjectTypeSOList.Count) % placedObjectTypeSOList.Count;

                placedObjectTypeSO = placedObjectTypeSOList[placedObjectTypeSOidx];
                UtilsClass.CreateWorldTextPopup(placedObjectTypeSO.name, Mouse3D.GetMouseWorldPosition());
                OnSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (placedObjectTypeSO == null) placedObjectTypeSOidx = 0;
                else placedObjectTypeSOidx = (placedObjectTypeSOidx + 1) % placedObjectTypeSOList.Count;
                placedObjectTypeSO = placedObjectTypeSOList[placedObjectTypeSOidx];
                UtilsClass.CreateWorldTextPopup(placedObjectTypeSO.name, Mouse3D.GetMouseWorldPosition());
                OnSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                placedObjectTypeSO = null;
                UtilsClass.CreateWorldTextPopup("exit build mode", Mouse3D.GetMouseWorldPosition());
                OnSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Vector3 GetMouseWorldSnappedPosition()
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePosition, out int x, out int z);

            if (placedObjectTypeSO != null)
            {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
                return placedObjectWorldPosition;
            }
            else
            {
                return mousePosition;
            }
        }

        public Quaternion GetPlacedObjectRotation()
        {
            if (placedObjectTypeSO != null)
            {
                return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
            }
            else
            {
                return Quaternion.identity;
            }
        }

        public PlacedObjectTypeSO GetPlacedObjectTypeSO()
        {
            return placedObjectTypeSO;
        }
    }

    public class GridObject
    {
        private GridXZ<GridObject> grid;
        private int x;
        private int z;
        private PlacedObject placedObject;
        public bool CanBuild => placedObject == null;

        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void SetPlacedObject(PlacedObject placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x,z);
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }
        public void ClearPlacedObject()
        {
            SetPlacedObject(null);
        }
        

        public override string ToString()
        {
            return x + " , " + z+"\n"+ placedObject;
        }
    }
}
