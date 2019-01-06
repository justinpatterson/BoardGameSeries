using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
	public GlitchEffect glitchEffectReference;
	public GLITCH_UIController glitchUIReference;

	public void TriggerGlitchEvent(GlitchEvent inputEvent)
	{
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

		glitchUIReference.DisplayGlitchPopUpMessage( inputEvent.message );

		while(glitchUIReference.messageInProgress) yield return new WaitForEndOfFrame();

		glitchEffectReference.enabled = false;
		glitchUIReference.HideGlitchPopUpMessage();
	}


	public class GlitchEvent
	{
		public string message;
		public float colorIntensity = 1f;
		public float flipIntensity = 1f;
		public float intensity = 1f;
		public Texture2D targetTextureDisplacementMap;
	}
}
