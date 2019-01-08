using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SETTINGS_GamePhaseBehavior : GamePhaseBehavior
{
	public override void StartPhase ()
	{
		base.StartPhase ();
		GameManager.OnBackClicked += TriggerBackClick;
	}	
	public void TriggerBackClick()
	{
		GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.start);
	}
	public override void EndPhase ()
	{
		GameManager.OnBackClicked -= TriggerBackClick;
		base.EndPhase ();
	}
}
