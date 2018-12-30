using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class END_GamePhaseBehavior : GamePhaseBehavior
{
    public override void StartPhase()
    {
        if (GameManager.instance && phaseUI)
        {
            if (phaseUI is END_UIController)
            {
                END_UIController phaseUI_cast_end = (END_UIController) phaseUI;
                phaseUI_cast_end.ReportResults(GameManager.instance.GetResults());
            }

            GameManager.OnBackClicked += TriggerBackClick;
            GameManager.OnRestartClicked += TriggerRestartClicked;
        }
        base.StartPhase();
    }
    public override void EndPhase()
    {
        base.EndPhase();
        GameManager.OnBackClicked -= TriggerBackClick;
    }
    public override void UpdatePhase()
    {
        base.UpdatePhase();
    }

    public void TriggerBackClick()
    {
        GameManager.instance.ticTacToeBoardReference.ClearBoard();
        GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.start);
    }

    public void TriggerRestartClicked()
    {
        GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.inGame);
    }
}
