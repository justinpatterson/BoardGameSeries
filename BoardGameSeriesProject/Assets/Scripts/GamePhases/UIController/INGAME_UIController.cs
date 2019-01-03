using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class INGAME_UIController : UIController 
{
	public Image currentPlayerNumberImage;
	public Image playerA_slot;
	public Image playerB_slot;
	Coroutine _playerSlotCoroutine;


	public override void OpenUI ()
	{
		base.OpenUI ();
	}
	public override void CloseUI ()
	{
		if(_playerSlotCoroutine != null) StopCoroutine( _playerSlotCoroutine );
		base.CloseUI ();
	}

	public void TriggerPlayerNumberImageUpdate(int inputPlayerNumber)
	{
		if(GameManager.instance)
		{
			if( inputPlayerNumber == -1 || inputPlayerNumber >= GameManager.instance.boardViewer.playerSprites.Length)
			{
			}
			else 
			{
				currentPlayerNumberImage.sprite = GameManager.instance.boardViewer.playerSprites[inputPlayerNumber];
				playerA_slot.sprite = GameManager.instance.boardViewer.playerSprites[0];
				playerB_slot.sprite = GameManager.instance.boardViewer.playerSprites[1];
				if(_playerSlotCoroutine != null) StopCoroutine( _playerSlotCoroutine );

				if(uiActive)
					_playerSlotCoroutine = StartCoroutine( SwapPlayerSlot( inputPlayerNumber ) );
			}
		}
	}
	public IEnumerator SwapPlayerSlot(int inputPlayerNumber)
	{
		float startTime = Time.unscaledTime;
		float targetDuration = 0.5f;
		float percentage = 0f;

		Vector2 startLocalPosition_playerA;
		Vector2 startLocalPosition_playerB;
		startLocalPosition_playerA = (inputPlayerNumber!=0) ? Vector3.zero : Vector3.up * -1f * 400f;
		startLocalPosition_playerB = (inputPlayerNumber!=1) ? Vector3.zero : Vector3.up * -1f * 400f;

		Vector2 desiredLocalPosition_playerA;
		Vector2 desiredLocalPosition_playerB;
		desiredLocalPosition_playerA = (inputPlayerNumber==0) ? Vector3.zero : Vector3.up * -1f * 400f;
		desiredLocalPosition_playerB = (inputPlayerNumber==1) ? Vector3.zero : Vector3.up * -1f * 400f;


		while( percentage < 1f )
		{
			percentage = ( Time.unscaledTime - startTime ) / targetDuration;
			percentage = Mathf.Clamp( percentage, 0f, 1f);
			playerA_slot.transform.localPosition = Vector3.Lerp( startLocalPosition_playerA, desiredLocalPosition_playerA, percentage);
			playerB_slot.transform.localPosition = Vector3.Lerp( startLocalPosition_playerB, desiredLocalPosition_playerB, percentage);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForEndOfFrame();
	}
}
