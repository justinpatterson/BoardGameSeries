using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlitchEvent
{
	[Header("Event Effect Values")]
	public string message;
	public float colorIntensity = 1f;
	public float flipIntensity = 1f;
	public float intensity = 1f;

	[Header("Event Queue Requirements")]
	public GameManager.GamePhases eventTriggerGamePhase = GameManager.GamePhases.inGame;
	public bool hasTriggered = false;

	[Header("Event Completion Requirements")]
	public Texture2D targetTextureDisplacementMap;
	public enum EventTriggerType {boardOrder, boardConfiguration, elapsedTimeInMode }
	public EventTriggerType eventTriggerListener = EventTriggerType.boardOrder;


	//board order 
	//board configuration
	public Vector2[] boardPositionOrder;

	//elapsed time
	public float targetElapsedTime = 10f;
	float _eventStartTime = 0f;

	public void StartGlitchEventListener()
	{
		Debug.Log("GE Listener Subscribing to Delegates.");
		switch(eventTriggerListener)
		{
		case EventTriggerType.boardOrder:
		case EventTriggerType.boardConfiguration:
			{
				BoardModel.OnBoardUpdated += DoEventCheck_BoardState;
				GameManager.OnBackClicked += EndEventListener;
			}
			break;
		case EventTriggerType.elapsedTimeInMode:
			{
				_eventStartTime = Time.unscaledTime;
				GameManager.OnGameUpdated += DoEventCheck_OnUpdate;
				GameManager.OnBackClicked += EndEventListener;
			}
			break;
		}
	}

	void DoEventCheck_OnUpdate()
	{
		switch(eventTriggerListener)
		{
		case EventTriggerType.elapsedTimeInMode:
			{
				if( Time.unscaledTime > ( _eventStartTime + targetElapsedTime ) )
				{
					DoEvent();
				}
			}
			break;
		default:
			break;
		}
	}

	void DoEventCheck_BoardState(List<Vector2> inputBoardState)
	{
		switch(eventTriggerListener)
		{
		case EventTriggerType.boardOrder:
		case EventTriggerType.boardConfiguration:
			{
				int matchedBoardPositions = 0;
				for(int i = 0; i < boardPositionOrder.Length; i++)
				{
					if(i < inputBoardState.Count)
					{
						if( boardPositionOrder[i] == inputBoardState[i] ) 
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
		Debug.Log("GE Listener Unsubscribing from Delegates.");
		GameManager.OnBackClicked -= EndEventListener;
		if(eventTriggerListener == EventTriggerType.boardOrder || eventTriggerListener == EventTriggerType.boardConfiguration) 
			BoardModel.OnBoardUpdated -= DoEventCheck_BoardState;
		if(eventTriggerListener == EventTriggerType.elapsedTimeInMode) 
			GameManager.OnGameUpdated -= DoEventCheck_OnUpdate;
	}
}