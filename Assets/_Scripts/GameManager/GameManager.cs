using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum GameState
{
    Default,
    Construction,
}
[RequireComponent(typeof(PlayerInput))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool PointerOverUI { private set; get; }

    public GameState gameState = GameState.Default;
    private PlayerInput playerInput;

    private void Awake()
    {
        instance = this;
        playerInput = GetComponent<PlayerInput>();
        
    }
    private void FixedUpdate()
    {
        PointerOverUI = EventSystem.current.IsPointerOverGameObject();
    }
    public void SwitchToConstructionMode()
    {
        playerInput.SwitchCurrentActionMap("Construction");
        gameState = GameState.Construction;
    }

    public void SwitchToDefaultMode()
    {
        playerInput.SwitchCurrentActionMap("Default");
        ConstructionManager.instance.ResetSelectedGridObjectSO();
        gameState = GameState.Default;
    }

}
