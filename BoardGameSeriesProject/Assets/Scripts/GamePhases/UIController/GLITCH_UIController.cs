using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GLITCH_UIController : UIController
{

	public Text glitchText;
	public bool messageInProgress;

	public override void OpenUI ()
	{
		uiContainer.transform.localScale = Vector3.zero;
		glitchText.text = "";
		base.OpenUI ();
	}

	public void DisplayGlitchPopUpMessage( string inputMessage )
	{
		OpenUI();
		StartCoroutine( PopUpMessageCoroutine( inputMessage ) );
	}

	IEnumerator PopUpMessageCoroutine( string inputMessage)
	{
		messageInProgress = true;
		yield return StartCoroutine(OpenMessage_PopUpMessageCoroutine());
		yield return StartCoroutine(RevealText_PopUpMessageCoroutine(inputMessage));
		yield return new WaitForSecondsRealtime(2f);
		yield return StartCoroutine(CloseMessage_PopUpMessageCoroutine());
		messageInProgress = false;
	}

	public void HideGlitchPopUpMessage()
	{
		CloseUI();
	}


	IEnumerator OpenMessage_PopUpMessageCoroutine() 
	{
		float startTime = Time.unscaledTime;
		float targetDuration = 0.1f;
		float percentage = 0f;
		while( percentage < 1f )
		{
			percentage = ( Time.unscaledTime - startTime ) / targetDuration;
			percentage = Mathf.Clamp( percentage, 0f, 1f);
			uiContainer.transform.localScale = new Vector3(1f, (percentage), 1f);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
	}

	IEnumerator RevealText_PopUpMessageCoroutine(string inputMessage)
	{
		float startTime = Time.unscaledTime;
		float targetDuration = inputMessage.Length * 0.1f;
		float percentage = 0f;
		while( percentage < 1f )
		{
			percentage = ( Time.unscaledTime - startTime ) / targetDuration;
			percentage = Mathf.Clamp( percentage, 0f, 1f);
			float f_currentIndex = (percentage) * (inputMessage.Length);
			int i_currentIndex = (int) f_currentIndex;
			i_currentIndex = Mathf.Clamp(i_currentIndex, 0, inputMessage.Length);
			string s_currentIndex = inputMessage.Substring(0, i_currentIndex);
			glitchText.text = s_currentIndex;
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
	}

	IEnumerator CloseMessage_PopUpMessageCoroutine() 
	{
		float startTime = Time.unscaledTime;
		float targetDuration = 0.1f;
		float percentage = 0f;
		while( percentage < 1f )
		{
			percentage = ( Time.unscaledTime - startTime ) / targetDuration;
			percentage = Mathf.Clamp( percentage, 0f, 1f);
			uiContainer.transform.localScale = new Vector3(1f, (1f-percentage), 1f);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
	}
}
