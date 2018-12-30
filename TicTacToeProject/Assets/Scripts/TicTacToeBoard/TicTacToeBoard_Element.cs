using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeBoard_Element : MonoBehaviour {
	//public Sprite[] playerSpriteOptions;
	public SpriteRenderer playerSpriteSlot;
	public SpriteRenderer bgSprite;
	public Vector2 boardPositionAssignment;
	int _currentPlayerNumberOwner = -1;

	Coroutine _revealPlayerSpriteCoroutine;

	public void AssignPlayerSlot(int playerNumber)
	{
		if(GameManager.instance)
		{
			if( playerNumber == -1 || playerNumber >= GameManager.instance.ticTacToeBoardReference.boardViewer.playerSprites.Length)
			{
				playerSpriteSlot.enabled = false;
			}
			else 
			{
				playerSpriteSlot.sprite = GameManager.instance.ticTacToeBoardReference.boardViewer.playerSprites[playerNumber];
				_revealPlayerSpriteCoroutine = StartCoroutine( RevealPlayerNumberSpriteCoroutine() );
				playerSpriteSlot.enabled = true;
				_currentPlayerNumberOwner = playerNumber;
				bgSprite.color = Color.white * 1f;
			}
		}
	}
	public void AssignPosition(Vector2 inputPosition){ boardPositionAssignment = inputPosition; }

	void OnMouseDown()
	{
		if(GameManager.instance) GameManager.instance.ReportTicTacToeTilePressed(boardPositionAssignment);
	}
	void OnMouseEnter()
	{
		if( _currentPlayerNumberOwner == -1 ) 
		{
			bgSprite.color = Color.white * 0.8f;
		}
	}
	void OnMouseExit()
	{
		if( _currentPlayerNumberOwner == -1 ) 
		{
			bgSprite.color = Color.white * 1f;
		}
	}

	void OnDestroy()
	{
		if(_revealPlayerSpriteCoroutine != null) StopCoroutine( _revealPlayerSpriteCoroutine );
	}

	IEnumerator RevealPlayerNumberSpriteCoroutine()
	{
		playerSpriteSlot.transform.localScale = Vector3.zero;
		playerSpriteSlot.transform.localScale = Vector3.zero;
		float startTime = Time.unscaledTime;
		float targetDuration = 0.2f;
		float percentage = 0f;
		while( percentage < 1f )
		{
			percentage = ( Time.unscaledTime - startTime ) / targetDuration;
			percentage = Mathf.Clamp( percentage, 0f, 1f);
			playerSpriteSlot.transform.localScale = Vector3.one * percentage;
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
	}
}
