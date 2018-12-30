using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class END_UIController : UIController {
    
	//public Image popUpBody_playerIMG;
	public Image[] popUp_playerIMGs;
	public GameObject popUp_container;
	public GameObject popUp_Winner_container;
	public GameObject popUp_Draw_container;

	Coroutine _activeCoroutine;

	public override void OpenUI ()
	{
		base.OpenUI ();
		_activeCoroutine = StartCoroutine( DisplayPopupCoroutine() );
	}

	public override void CloseUI ()
	{
		popUp_Draw_container.SetActive(false);
		popUp_Winner_container.SetActive(false);
		if(_activeCoroutine != null) 
		{
			StopCoroutine( _activeCoroutine );
			_activeCoroutine = null;
		}
		base.CloseUI ();
	}

	public void ReportResults(Results inputResults)
    {
		if(inputResults.winningPlayerNumber != -1) 
		{
			Sprite winningPlayerSprite = GameManager.instance.ticTacToeBoardReference.boardViewer.playerSprites[ inputResults.winningPlayerNumber ];
			foreach(Image playerIMG in popUp_playerIMGs) playerIMG.sprite = winningPlayerSprite;
			popUp_Winner_container.SetActive(true);
		}
		else 
		{
			popUp_Draw_container.SetActive(true);
			Debug.Log("container HAS BEEN ENABLED");
		}
    }

	IEnumerator DisplayPopupCoroutine()
	{
		popUp_container.transform.localScale = Vector3.zero;
		float startTime = Time.unscaledTime;
		float targetDuration = 0.3f;
		float percentage = 0f;
		while( percentage < 1f )
		{
			percentage = ( Time.unscaledTime - startTime ) / targetDuration;
			percentage = Mathf.Clamp( percentage, 0f, 1f);
			popUp_container.transform.localScale = Vector3.one * percentage;
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
	}

}
