using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class INGAME_UIController : UIController 
{
	public Image currentPlayerNumberImage;
	public Image playerA_slot;
	public Image playerB_slot;
	public Text playerA_text;
	public Text playerB_text;

	Coroutine _playerSlotCoroutine;


	public override void OpenUI ()
	{
		if(playerA_text) playerA_text.text = PlayerPrefs.GetString( PlayerDataController.PlayerDataTypes.p1_name.ToString() );
		if(playerB_text) playerB_text.text = PlayerPrefs.GetString( PlayerDataController.PlayerDataTypes.p2_name.ToString() );
		base.OpenUI ();
	}
	public override void CloseUI ()
	{
		if(_playerSlotCoroutine != null) StopCoroutine( _playerSlotCoroutine );
		base.CloseUI ();
	}

	public void TriggerPlayerNumberImageUpdate(int inputPlayerNumber, bool useAnim = true)
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
					_playerSlotCoroutine = StartCoroutine( SwapPlayerSlot( inputPlayerNumber, ( useAnim ? 0.5f:0f) ) );
			}
		}
	}
	public IEnumerator SwapPlayerSlot(int inputPlayerNumber, float targetDuration)
	{
		float startTime = Time.unscaledTime;
		float percentage = 0f;

		Vector2 startLocalPosition_playerA;
		Vector2 startLocalPosition_playerB;
		startLocalPosition_playerA = (inputPlayerNumber!=0) ? Vector3.zero : Vector3.up * -1f * 400f;
		startLocalPosition_playerB = (inputPlayerNumber!=1) ? Vector3.zero : Vector3.up * -1f * 400f;

		Vector2 desiredLocalPosition_playerA;
		Vector2 desiredLocalPosition_playerB;
		desiredLocalPosition_playerA = (inputPlayerNumber==0) ? Vector3.zero : Vector3.up * -1f * 400f;
		desiredLocalPosition_playerB = (inputPlayerNumber==1) ? Vector3.zero : Vector3.up * -1f * 400f;

		if(targetDuration == 0f)
		{
			playerA_slot.transform.localPosition = desiredLocalPosition_playerA;
			playerB_slot.transform.localPosition = desiredLocalPosition_playerB;
		}
		else 
		{
			while( percentage < 1f )
			{
				percentage = ( Time.unscaledTime - startTime ) / targetDuration;
				percentage = Mathf.Clamp( percentage, 0f, 1f);
				playerA_slot.transform.localPosition = Vector3.Lerp( startLocalPosition_playerA, desiredLocalPosition_playerA, percentage);
				playerB_slot.transform.localPosition = Vector3.Lerp( startLocalPosition_playerB, desiredLocalPosition_playerB, percentage);
				yield return new WaitForEndOfFrame();
			}	
		}


		yield return new WaitForEndOfFrame();
		_playerSlotCoroutine = null;
	}
}
