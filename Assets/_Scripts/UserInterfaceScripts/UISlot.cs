using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UISlot : MonoBehaviour
{
    public void Populate(GridObject gridObject, Action<UISlot> onClickAction)
    {
        this.gridObject = gridObject;
        if(gridObject != null)
        {
            Icon.sprite = gridObject.icon;
            Name.text = gridObject.nameString;
        }
        OnClickAction = onClickAction;
    }

    public Button Button;
    public Image Background;
    public Image Icon;
    public TextMeshProUGUI Name;

    public GridObject gridObject;

    private Action<UISlot> OnClickAction;
    public void OnClick()
    {
        OnClickAction?.Invoke(this);
    }
    
}
