using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HUD : MonoBehaviour
{
    [SerializeField] private UISlot UISlotPrefab;
    protected static SimulationPanel ui => SimulationPanel.instance;
    protected static CafeSimulationManager cafe => CafeSimulationManager.instance;
    public abstract void Open(BaseBehaviour data);
    public abstract void Refresh();
    public abstract void Close();
    protected UISlot CreateSlot(Transform container)
    {
        GameObject createdUISlot = Instantiate(UISlotPrefab.gameObject, container);
        return createdUISlot.GetComponent<UISlot>();
    }

}
