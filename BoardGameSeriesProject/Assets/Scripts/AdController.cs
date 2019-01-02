using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour
{
	#if UNITY_IOS
	private string gameId = "2986613";
	private bool useAds = true;
	#elif UNITY_ANDROID
	private string gameId = "2986613";
	private bool useAds = true;
	#else
	private string gameId = "";
	private bool useAds = false;
	#endif

	public delegate void RewardAdCompletedAction(ShowResult result);
	public static event RewardAdCompletedAction OnRewardAdCompleted;

	public void InitializeAds()
	{
		if(!useAds) return;
		Advertisement.Initialize(gameId,true);
	}
	public void ShowRewardedAd () {
		if(!useAds) return;
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}
	private void HandleShowResult(ShowResult result)
	{

		if( OnRewardAdCompleted != null ) 
		{
			OnRewardAdCompleted(result);
		}
		else {Debug.Log("No ad listeners.");}
	}
}
