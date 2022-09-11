using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimulationPanel : MonoBehaviour
{
    public static SimulationPanel instance;
    private CafeSimulationManager cafe;

    [SerializeField]private CookingOvenHUD cookingOvenHUD;
    [SerializeField]private AssignChefHUD assignChefHUD;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cafe = CafeSimulationManager.instance;
    }
    public ChefBehaviour GetSelectedChef()
    {
        return assignChefHUD.selectedChef;
    }
    public void OpenHUD(CookingOvenBehaviour cookingOven)
    {
        cookingOvenHUD.Open(cookingOven);
    }
    public void OpenHUD(ChefBehaviour selectedChef)
    {
        assignChefHUD.Open(selectedChef);
    }
    public void CloseHud()
    {

    }
}
