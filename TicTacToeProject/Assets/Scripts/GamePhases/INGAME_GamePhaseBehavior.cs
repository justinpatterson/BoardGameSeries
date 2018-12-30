using System.Collections;
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
					(GameManager.instance.ticTacToeBoardReference.width * GameManager.instance.ticTacToeBoardReference.boardViewer.spriteSize) / 2,
					(GameManager.instance.ticTacToeBoardReference.width * GameManager.instance.ticTacToeBoardReference.boardViewer.spriteSize) / 2,
                Camera.main.transform.position.z
                );
			GameManager.instance.ticTacToeBoardReference.Init();
			GameManager.OnTileClicked += TriggerTileClick;
            GameManager.OnBackClicked += TriggerBackClick;
			currentSubPhase = InGameSubPhases.player1_turn;
			ReportCurrentPlayerTurn(0);
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
//            Debug.Log("Position: " + position);
			if ( GameManager.instance.ticTacToeBoardReference.PlayerClaimsPosition( 0, position) )
			{
				GameManager.instance.ticTacToeBoardReference.boardViewer.PlayerClaimedGridAtPosition( 0, position );
                if (GameManager.instance.ticTacToeBoardReference.CheckWinState())
                {
                    GameManager.instance.TriggerResultsGeneration(0);
                    GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.end);
				}
				else if(GameManager.instance.ticTacToeBoardReference.GetCurrentTurnCount() >= (Mathf.Pow(GameManager.instance.ticTacToeBoardReference.width,2)))
				{

					GameManager.instance.TriggerResultsGeneration(-1);
					GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.end);
				}
				currentSubPhase = InGameSubPhases.player2_turn;
				ReportCurrentPlayerTurn(1);

            }
                
			break;
		case InGameSubPhases.player2_turn:
            Debug.Log("Position: " + position);
            if (GameManager.instance.ticTacToeBoardReference.PlayerClaimsPosition(1, position))
            {
                GameManager.instance.ticTacToeBoardReference.boardViewer.PlayerClaimedGridAtPosition(1, position);
                if (GameManager.instance.ticTacToeBoardReference.CheckWinState())
                {
                    GameManager.instance.TriggerResultsGeneration(1);
                    GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.end);
                }
				else if(GameManager.instance.ticTacToeBoardReference.GetCurrentTurnCount() >= (Mathf.Pow(GameManager.instance.ticTacToeBoardReference.width,2)))
				{
					GameManager.instance.TriggerResultsGeneration(-1);
					GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.end);
				}
				currentSubPhase = InGameSubPhases.player1_turn;
				ReportCurrentPlayerTurn(0);

            }
            break;
		}
	}
	void ReportCurrentPlayerTurn(int inputPlayerNumber)
	{
		if(phaseUI is INGAME_UIController)
		{
			INGAME_UIController phaseUI_cast = (INGAME_UIController) phaseUI;
			phaseUI_cast.TriggerPlayerNumberImageUpdate(inputPlayerNumber);
		}
	}

    public void TriggerBackClick()
    {
        GameManager.instance.ticTacToeBoardReference.ClearBoard();
        GameManager.instance.TriggerPhaseTransition(GameManager.GamePhases.start);
    }
}
