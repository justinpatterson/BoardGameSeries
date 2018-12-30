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

	public override void CloseUI ()
	{
		base.CloseUI ();
	}

    public void SetSharedUIDisplay (SharedUISettings inputSharedSettings)
    {
		if(!uiActive) OpenUI();
        topBannerContainer.SetActive(inputSharedSettings.showTopBannerBG);
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

}

[System.Serializable]
public class SharedUISettings
{
    public bool showTopBannerBG = true;
    public bool backButton;
	public bool overlay;
}

