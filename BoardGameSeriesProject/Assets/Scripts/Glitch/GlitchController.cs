using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
	public GlitchEffect glitchEffectReference;
	public GLITCH_UIController glitchUIReference;
	[SerializeField]
	public GlitchEvent[] glitchEvents;
	int _currentGlitchEventIndex = -1;

	bool _forceGlitches = true;

	public void Init()
	{
		if(_forceGlitches) return;
		foreach(GlitchEvent g in glitchEvents)
		{
			int glitchStatus = PlayerPrefs.GetInt( g.playerPrefKey, 0);
			if(glitchStatus == 1) g.hasTriggered = true;
		}
	}


	public void QueueNextGlitchEventListener()
	{

		if(_currentGlitchEventIndex != -1)
		{
			glitchEvents[_currentGlitchEventIndex].EndEventListener();
			_currentGlitchEventIndex = -1;
		}

		for(int i = 0; i < glitchEvents.Length; i++)
		{
			//Debug.Log("Event ... " + i);
			if(glitchEvents[i].hasTriggered == false)
			{
				//Debug.Log("HAS NOT TRIGGERED YET...");
				//Debug.Log(glitchEvents[i].eventTriggerGamePhase.ToString() + " VERSUS " + GameManager.instance.currentPhase.ToString() );
				if(glitchEvents[i].eventTriggerGamePhase == GameManager.instance.currentPhase)
				{
					
					_currentGlitchEventIndex = i;
					glitchEvents[i].StartGlitchEventListener();
					break;
				}
			}
		}	
	}


	public void TriggerGlitchEvent(GlitchEvent inputEvent)
	{
		_currentGlitchEventIndex = -1;
		PlayerPrefs.SetInt( inputEvent.playerPrefKey, 1 );
		StartCoroutine( GlitchEventCoroutine( inputEvent ) );
	}

	IEnumerator GlitchEventCoroutine(GlitchEvent inputEvent)
	{
		glitchEffectReference.colorIntensity = inputEvent.colorIntensity;
		glitchEffectReference.flipIntensity = inputEvent.flipIntensity;
		glitchEffectReference.intensity = inputEvent.intensity;
		glitchEffectReference.enabled = true;


		if(inputEvent.targetTextureDisplacementMap!=null) 
			glitchEffectReference.displacementMap = inputEvent.targetTextureDisplacementMap;

		if(inputEvent.message.Length>0) 
		{
			glitchUIReference.DisplayGlitchPopUpMessage( inputEvent.message );
			while(glitchUIReference.messageInProgress) yield return new WaitForEndOfFrame();
		}
		else
		{
			yield return new WaitForSeconds(0.2f);
		}

		glitchEffectReference.enabled = false;
		glitchUIReference.HideGlitchPopUpMessage();
	}


}
