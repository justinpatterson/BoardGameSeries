using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
	public GlitchEffect glitchEffectReference;
	public GLITCH_UIController glitchUIReference;
	[SerializeField]
	public GlitchEvent glitchEvent;

	void Start()
	{
		glitchEvent.StartEventListener();
	}

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

	[System.Serializable]
	public class GlitchEvent
	{
		[Header("Event Effect Values")]
		public string message;
		public float colorIntensity = 1f;
		public float flipIntensity = 1f;
		public float intensity = 1f;
		[Header("Event Requirements")]
		public Texture2D targetTextureDisplacementMap;
		public enum EventTriggerType {boardOrder, boardConfiguration /*etc*/ }
		public EventTriggerType eventTriggerListener = EventTriggerType.boardOrder;
		public bool hasTriggered = false;
		public Vector2[] boardPositionOrder;

		public void StartEventListener()
		{
			switch(eventTriggerListener)
			{
				case EventTriggerType.boardOrder:
				case EventTriggerType.boardConfiguration:
					{
					BoardModel.OnBoardUpdated += DoEventCheck_BoardState;
					GameManager.OnBackClicked += EndEventListener;
					Debug.Log("Glitch Event listener active...");
						//GameManager.OnYada += DoEventCheck;
					}
				break;
			}
		}

		void DoEventCheck_BoardState(Dictionary<Vector2, int> inputCurrentBoardSate, List<Vector2> inputBoardTurnHistory)
		{
			switch(eventTriggerListener)
			{
			case EventTriggerType.boardOrder:
			case EventTriggerType.boardConfiguration:
				{
					Debug.Log("Glitch Event will now fire");
					int matchedBoardPositions = 0;
					for(int i = 0; i < boardPositionOrder.Length; i++)
					{
						if(i < inputBoardTurnHistory.Count)
						{
							if( boardPositionOrder[i] == inputBoardTurnHistory[i] ) 
							{
								matchedBoardPositions++;
							}
							else 
							{
								if(eventTriggerListener == EventTriggerType.boardOrder) matchedBoardPositions = 0;
								break;
							}
						}
					}
					if(matchedBoardPositions == boardPositionOrder.Length) 
					{
						DoEvent();
					}
					else if(matchedBoardPositions > 0)
					{
						GlitchEvent g = new GlitchEvent();
						g.boardPositionOrder = this.boardPositionOrder;
						g.colorIntensity = this.colorIntensity;
						g.flipIntensity = this.flipIntensity;
						g.message = "";
						GameManager.instance.glitchController.TriggerGlitchEvent( g );
					}
				}
				break;
			default:
				Debug.Log("Unsupported glitch event listener for Board State");
				EndEventListener();
				break;
			}
		}

		void DoEvent()
		{
			GameManager.instance.glitchController.TriggerGlitchEvent( this );
			this.hasTriggered = true;
			EndEventListener();
		}
		public void EndEventListener()
		{
			Debug.Log("Glitch listener deactivating.");
			GameManager.OnBackClicked -= EndEventListener;
			BoardModel.OnBoardUpdated -= DoEventCheck_BoardState;
		}
	}
}
