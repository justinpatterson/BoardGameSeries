using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

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
        GameManager.instance.boardModel.ClearBoard();
        GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.start);
    }

    public void TriggerRestartClicked()
    {
        GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.inGame);
		ShowRewardedAd();
    }
	void ShowRewardedAd () {
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
		/*
		AdvertisementShowAdCallbacks options = new ShowAdCallbacks ();
		options.finishCallback = HandleShowResult;
		ShowAdPlacementContent ad = Monetization.GetPlacementContent (placementId) as ShowAdPlacementContent;
		ad.Show (options);
		*/
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
}
