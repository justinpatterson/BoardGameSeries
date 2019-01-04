﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INGAME_GamePhaseBehavior : GamePhaseBehavior
{
    public enum InGameSubPhases { player1_turn, player2_turn }
    public InGameSubPhases currentSubPhase = InGameSubPhases.player1_turn;

    public override void StartPhase()
    {
        base.StartPhase();

		if (GameManager.instance)
		{
			Camera.main.transform.position = new Vector3
				(
					(GameManager.instance.boardViewer.spriteSize) * (GameManager.instance.boardModel.width/2),
					(GameManager.instance.boardModel.height * GameManager.instance.boardViewer.spriteSize) / 2,
					Camera.main.transform.position.z
				);
			Camera.main.orthographicSize =GameManager.instance.boardViewer.spriteSize *
				((GameManager.instance.boardModel.width>GameManager.instance.boardModel.height) ? 
					GameManager.instance.boardModel.width + .5f 
					: GameManager.instance.boardModel.height + .5f);
			GameManager.instance.boardModel.Init();
			GameManager.OnTileClicked += TriggerTileClick;
            GameManager.OnBackClicked += TriggerBackClick;
			currentSubPhase = InGameSubPhases.player1_turn;
			ReportCurrentPlayerTurn(0,false);
		}
    }
    public override void UpdatePhase()
    {
        base.UpdatePhase();

    }
    public override void EndPhase()
    {
        base.EndPhase();
		GameManager.OnTileClicked -= TriggerTileClick;
        GameManager.OnBackClicked -= TriggerBackClick;
    }

	public void TriggerTileClick(Vector2 position)
	{
		switch(currentSubPhase)
		{
		case InGameSubPhases.player1_turn:
            Vector2 position_clean_p1 = GameManager.instance.boardModel.GetNextPlayerPosition(position);
            if ( GameManager.instance.boardModel.PlayerClaimsPosition( 0, position_clean_p1) )
			{
				GameManager.instance.boardViewer.PlayerClaimedGridAtPosition( 0, position_clean_p1);
                if (GameManager.instance.boardModel.CheckWinState())
                {
                    GameManager.instance.TriggerResultsGeneration(0);
                    GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.end);
				}
				else if(GameManager.instance.boardModel.GetCurrentTurnCount() >= (Mathf.Pow(GameManager.instance.boardModel.width,2)))
				{

					GameManager.instance.TriggerResultsGeneration(-1);
					GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.end);
				}
				currentSubPhase = InGameSubPhases.player2_turn;
				ReportCurrentPlayerTurn(1,true);

            }
                
			break;
		case InGameSubPhases.player2_turn:
            Vector2 position_clean_p2 = GameManager.instance.boardModel.GetNextPlayerPosition(position);
            if (GameManager.instance.boardModel.PlayerClaimsPosition(1, position_clean_p2))
            {
                GameManager.instance.boardViewer.PlayerClaimedGridAtPosition(1, position_clean_p2);
                if (GameManager.instance.boardModel.CheckWinState())
                {
                    GameManager.instance.TriggerResultsGeneration(1);
                    GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.end);
                }
				else if(GameManager.instance.boardModel.GetCurrentTurnCount() >= (Mathf.Pow(GameManager.instance.boardModel.width,2)))
				{
					GameManager.instance.TriggerResultsGeneration(-1);
					GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.end);
				}
				currentSubPhase = InGameSubPhases.player1_turn;
				ReportCurrentPlayerTurn(0,true);
            }
            break;
		}
	}
	void ReportCurrentPlayerTurn(int inputPlayerNumber, bool inputUseAnim)
	{
		if(phaseUI is INGAME_UIController)
		{
			INGAME_UIController phaseUI_cast = (INGAME_UIController) phaseUI;
			phaseUI_cast.TriggerPlayerNumberImageUpdate(inputPlayerNumber, inputUseAnim);
		}
	}

    public void TriggerBackClick()
    {
        GameManager.instance.boardModel.ClearBoard();
        GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.start);
    }
}
