using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBehaviour : MonoBehaviour
{
    protected CafeSimulationManager cafe;
    public PlacedObject placedObject;
    public string Name => placedObject.placedObjectTypeSO.nameString;
    public Sprite Icon => placedObject.placedObjectTypeSO.icon;

    protected virtual void Awake()
    {
        Debug.Log("BaseBehaviour Awake!");
        placedObject = GetComponent<PlacedObject>();
        cafe = CafeSimulationManager.instance;
    }

    public abstract void OnClick();
    public GridObject GetData()
    {
        return placedObject.placedObjectTypeSO;
    }
}
