﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class START_UIController : UIController {
    public Button startGameButton;

	public Toggle Toggle_3x3, Toggle_4x4;
	public InputField inputName_p1, inputName_p2;

	public Transform boardModeContainer;

    public override void OpenUI()
    {
        base.OpenUI();
        if (GameManager.instance && startGameButton)
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(() => GameManager.instance.ReportGameStartPressed());
        }
		SetBoardModeSelection(0);
		Toggle_3x3.onValueChanged.RemoveAllListeners();
		Toggle_3x3.onValueChanged.AddListener( delegate { ReportGridSizeToggleOptionClicked(); } );
		Toggle_4x4.onValueChanged.RemoveAllListeners();
		Toggle_4x4.onValueChanged.AddListener( delegate { ReportGridSizeToggleOptionClicked(); } );

		inputName_p1.text = PlayerPrefs.GetString( PlayerDataController.PlayerDataTypes.p1_name.ToString() );
		inputName_p2.text = PlayerPrefs.GetString( PlayerDataController.PlayerDataTypes.p2_name.ToString() );

		inputName_p1.onEndEdit.RemoveAllListeners();
		inputName_p1.onEndEdit.AddListener( delegate { ReportPlayerNameFieldChange(0); });
		inputName_p2.onEndEdit.RemoveAllListeners();
		inputName_p2.onEndEdit.AddListener( delegate { ReportPlayerNameFieldChange(1); });

		int childIndex = 0;
		foreach(Transform child in boardModeContainer)
		{
			EventTrigger eTrig = child.GetComponent<EventTrigger>();
			if(eTrig) 
			{
				if(childIndex < GameManager.instance.boards.Length)
				{
					int currIndex = childIndex;
					eTrig.triggers.Clear();
					EventTrigger.Entry entry = new EventTrigger.Entry();
					entry.eventID = EventTriggerType.PointerClick;
					entry.callback.AddListener((data) => { OnPointerDownBoardOptionDelegate((PointerEventData)data, currIndex); });
					eTrig.triggers.Add(entry);

					BoardInfo bInfo = GameManager.instance.boards[childIndex];
					child.GetComponentInChildren<Text>().text = bInfo.boardName;
				}
				else 
				{
					child.GetComponentInChildren<Text>().text ="?";
				}
				childIndex++;
			}

		}
    }
    public override void CloseUI()
    {
        base.CloseUI();
    }

	public void ReportGridSizeToggleOptionClicked()
	{
		if(Toggle_3x3.isOn)
		{
			PlayerPrefs.SetInt( "GridSize", 3);
		}
		else if(Toggle_4x4.isOn)
		{
			PlayerPrefs.SetInt( "GridSize", 4);
		}
	}

	public void OnPointerDownBoardOptionDelegate(PointerEventData data, int inputChildIndex)
	{
		GameManager.instance.ReportBoardOptionPressed(inputChildIndex);
		SetBoardModeSelection(inputChildIndex);
	}

	void SetBoardModeSelection(int inputChildIndex)
	{
		int childIndex = 0;
		foreach(Transform child in boardModeContainer)
		{
			child.transform.localScale = Vector3.one * ((childIndex==inputChildIndex)? 1f:.9f);
			Image im = child.GetComponent<Image>();
			if(im)
			{
				im.color = Color.white * ((childIndex==inputChildIndex)? 1f:.9f);
			}
			childIndex++;
		}
	}

	void ReportPlayerNameFieldChange( int inputPlayer )
	{
		if(inputPlayer == 0)
		{
			GameManager.instance.playerDataController.SetPlayerDataValue( PlayerDataController.PlayerDataTypes.p1_name, inputName_p1.text );
		}
		else if(inputPlayer == 1)
		{
			GameManager.instance.playerDataController.SetPlayerDataValue( PlayerDataController.PlayerDataTypes.p2_name, inputName_p2.text );
		}
	}
}
