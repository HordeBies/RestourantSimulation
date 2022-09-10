using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Helper class to redirect raycast hit from child to parent
public class RaycastTarget : MonoBehaviour
{
    [SerializeField] private BaseBehaviour behaviour;
    private void Awake()
    {
        if(behaviour == null)
        {
            throw new System.Exception("This Raycast Target doesn't have a behaviour attached");
        }
    }
    public void OnClick()
    {
        behaviour.OnClick();
    }
    public GridObject GetData()
    {
        return behaviour.GetData();
    }
}
