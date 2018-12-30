using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhaseBehavior : MonoBehaviour {
    public GameManager.GamePhases phase;
    public bool phaseActive = false;
    public UIController phaseUI;

    public SharedUISettings sharedUI;

    public virtual void StartPhase()
    {
        phaseActive = true;
        if (phaseUI) phaseUI.OpenUI();
    }
    public virtual void EndPhase()
    {
        phaseActive = false;
        if (phaseUI) phaseUI.CloseUI();
    }
    public virtual void UpdatePhase()
    {
    }
}
