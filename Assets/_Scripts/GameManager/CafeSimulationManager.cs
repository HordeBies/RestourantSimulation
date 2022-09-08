using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CafeSimulationManager : MonoBehaviour
{
    public static CafeSimulationManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || GameManager.PointerOverUI) return;

        Debug.Log("Left Click on Default outside UI");
    }
}
