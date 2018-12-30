using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class START_UIController : UIController {
    public Button startGameButton;

	public Toggle Toggle_3x3, Toggle_4x4;

    public override void OpenUI()
    {
        base.OpenUI();
        if (GameManager.instance && startGameButton)
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(() => GameManager.instance.ReportGameStartPressed());
        }

		Toggle_3x3.onValueChanged.RemoveAllListeners();
		Toggle_3x3.onValueChanged.AddListener( delegate { ReportGridSizeToggleOptionClicked(); } );
		Toggle_4x4.onValueChanged.RemoveAllListeners();
		Toggle_4x4.onValueChanged.AddListener( delegate { ReportGridSizeToggleOptionClicked(); } );
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
}
