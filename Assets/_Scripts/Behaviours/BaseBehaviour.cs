using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    protected CafeSimulationManager cafe;
    public PlacedObject placedObject;
    public string Name => placedObject.placedObjectTypeSO.nameString;
    public Sprite Icon => placedObject.placedObjectTypeSO.icon;

    protected virtual void Awake()
    {
        placedObject = GetComponent<PlacedObject>();
        cafe = CafeSimulationManager.instance;
    }

    public virtual void OnClick()
    {
        Debug.Log("Clicked on an EmptyBehaviour");
    }
    public GridObject GetData()
    {
        return placedObject.placedObjectTypeSO;
    }
    public virtual void Load(BaseBehaviour behaviour)
    {
        Debug.Log("Loading Behaviour!");
    }
}
