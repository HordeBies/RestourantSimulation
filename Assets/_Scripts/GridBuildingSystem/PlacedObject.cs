using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour {

    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, GridObject.Dir dir, GridObject placedObjectTypeSO) {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
        placedObject.Setup(placedObjectTypeSO, origin, dir);

        return placedObject;
    }


    public ObjectType Type => placedObjectTypeSO.Type;
    public GridObject placedObjectTypeSO;
    public Vector2Int origin;
    public GridObject.Dir dir;
    public bool IsNPC => Type == ObjectType.Chef || Type == ObjectType.Server || Type == ObjectType.Customer;
    private void Setup(GridObject placedObjectTypeSO, Vector2Int origin, GridObject.Dir dir) {
        this.placedObjectTypeSO = placedObjectTypeSO;
        this.origin = origin;
        this.dir = dir;
    }
    public void Move(Vector3 toWorld, Vector2Int toGrid)
    {
        transform.position = toWorld;
        origin = toGrid;
    }
    public void Rotate(GridObject.Dir to)
    {
        dir = to;
    }

    public List<Vector2Int> GetGridPositionList() {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }
    public Vector2Int GetAdjacentTilePos(GridObject.Dir at)
    {
        switch (at)
        {
            default:
            case GridObject.Dir.Down: return new(origin.x, origin.y - 1);
            case GridObject.Dir.Left: return new(origin.x - 1, origin.y);
            case GridObject.Dir.Up: return new(origin.x, origin.y + 1);
            case GridObject.Dir.Right: return new(origin.x + 1, origin.y);
        }
    }
    public Vector2Int GetFrontTilePos()
    {
        return GetAdjacentTilePos(dir);
    }
    public void DestroySelf() {
        Destroy(gameObject);
    }

    public override string ToString() {
        return placedObjectTypeSO.nameString;
    }

}
