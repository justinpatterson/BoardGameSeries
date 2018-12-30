using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class INGAME_UIController : UIController 
{
	public Image currentPlayerNumberImage;

	public void TriggerPlayerNumberImageUpdate(int inputPlayerNumber)
	{
		if(GameManager.instance)
		{
			if( inputPlayerNumber == -1 || inputPlayerNumber >= GameManager.instance.ticTacToeBoardReference.boardViewer.playerSprites.Length)
			{
			}
			else 
			{
				currentPlayerNumberImage.sprite = GameManager.instance.ticTacToeBoardReference.boardViewer.playerSprites[inputPlayerNumber];
			}
		}
	}
}
