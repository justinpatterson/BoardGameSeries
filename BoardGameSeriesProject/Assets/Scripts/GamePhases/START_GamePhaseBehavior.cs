using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class START_GamePhaseBehavior : GamePhaseBehavior {

    public override void StartPhase()
    {
        base.StartPhase();
    }
    public override void UpdatePhase()
    {
        base.UpdatePhase();
    }
    public override void EndPhase()
    {
        base.EndPhase();
    }

	/*
	IEnumerator ShowBannerWhenReady()
	{
		while (!Advertisement.IsReady("banner"))
		{
			yield return new WaitForSeconds(0.5f);
		}

		Advertisem.Show("banner");
		yield return new WaitForEndOfFrame();
	}
	*/
}
