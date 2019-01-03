using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHARED_UIController : UIController {
    public GameObject topBannerContainer;
    public Button backButton;

	public GameObject overlayContainer;
	public Image overlayBG;

	Coroutine _overlayCoroutine;
	Coroutine _topBannerCoroutine;

	public override void CloseUI ()
	{
		if(_overlayCoroutine != null) StopCoroutine(_overlayCoroutine);
		if(_topBannerCoroutine != null) StopCoroutine(_topBannerCoroutine);
		base.CloseUI ();
	}

    public void SetSharedUIDisplay (SharedUISettings inputSharedSettings)
    {
		if(!uiActive) OpenUI();

		_topBannerCoroutine = StartCoroutine(DisplayTopBannerCoroutine(inputSharedSettings.showTopBannerBG));
        //topBannerContainer.SetActive(inputSharedSettings.showTopBannerBG);
        backButton.gameObject.SetActive(inputSharedSettings.backButton);
		overlayContainer.SetActive( inputSharedSettings.overlay );
		if( inputSharedSettings.overlay )
		{
			_overlayCoroutine = StartCoroutine(DispalyOverlayCoroutine());
		}
    }

	IEnumerator DispalyOverlayCoroutine()
	{
		overlayBG.color = new Color(0f,0f,0f,0f);
		float startTime = Time.unscaledTime;
		float targetDuration = 0.3f;
		float percentage = 0f;
		while( percentage < 1f )
		{
			percentage = ( Time.unscaledTime - startTime ) / targetDuration;
			percentage = Mathf.Clamp( percentage, 0f, 1f);
			overlayBG.color = new Color(0f,0f,0f,percentage*0.5f);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
	}

	IEnumerator DisplayTopBannerCoroutine(bool isDisplayed)
	{
		
		float startTime = Time.unscaledTime;
		float targetDuration = 0.2f;
		float percentage = 0f;
		while( percentage < 1f )
		{
			percentage = ( Time.unscaledTime - startTime ) / targetDuration;
			percentage = Mathf.Clamp( percentage, 0f, 1f);
			topBannerContainer.transform.localScale = new Vector3(1f, isDisplayed ? percentage:(1f - percentage), 1f);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
	}
}

[System.Serializable]
public class SharedUISettings
{
    public bool showTopBannerBG = true;
    public bool backButton;
	public bool overlay;
}

