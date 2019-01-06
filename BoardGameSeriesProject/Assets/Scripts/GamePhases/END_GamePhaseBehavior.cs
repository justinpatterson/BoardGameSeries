using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class END_GamePhaseBehavior : GamePhaseBehavior
{
    public override void StartPhase()
    {
		/*
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
        */
		StartCoroutine(StartPhaseCoroutine());
    }

	IEnumerator StartPhaseCoroutine()
	{
		WinVectorData w = GameManager.instance.boardModel.GetWinVectorData();
		if(w.isValid)
		{
			GameManager.instance.boardViewer.PlayerWinFX(
				w.start,
				w.end,
				0.6f
			);
			yield return new WaitForSeconds(1f);
		}
		yield return null;

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
        GameManager.instance.boardModel.ClearBoard();
		GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.start);
		AdController.OnRewardAdCompleted += TriggerOnRewardAdCompleted;
		GameManager.instance.adController.ShowRewardedAd();
    }

    public void TriggerRestartClicked()
    {
        GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.inGame);

    }

	public void TriggerOnRewardAdCompleted(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
		AdController.OnRewardAdCompleted -= TriggerOnRewardAdCompleted;
	}

}
