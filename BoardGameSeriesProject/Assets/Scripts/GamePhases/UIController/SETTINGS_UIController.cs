using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SETTINGS_UIController : UIController
{

	public Slider volumeSlider, sfxSlider, firewallSlider, resetSlider;

	public Image firewall_IMG;
	public Sprite firewall_off, firewall_off_alt, firewall_on;
	bool _firewallOff;

	public EventTrigger resetEventTrigger;
	bool _resetPressed;
	float _resetTimer = 0f;

	public override void OpenUI ()
	{
		ActivateSliderListeners();
		LoadSettingValues();
		base.OpenUI ();
	}

	void LoadSettingValues()
	{
		volumeSlider.value = PlayerPrefs.GetFloat( GameDataModel.GameSettings.volume.ToString(), 0.5f );
		sfxSlider.value = PlayerPrefs.GetFloat( GameDataModel.GameSettings.sound.ToString(), 0.5f );
		firewallSlider.value = PlayerPrefs.GetFloat( GameDataModel.GameSettings.firewall.ToString(), 1f );
	}

	void ActivateSliderListeners()
	{
		volumeSlider.onValueChanged.RemoveAllListeners();
		sfxSlider.onValueChanged.RemoveAllListeners();
		firewallSlider.onValueChanged.RemoveAllListeners();
		resetEventTrigger.triggers.Clear();

		volumeSlider.onValueChanged.AddListener( delegate { TriggerSliderUpdate( GameDataModel.GameSettings.volume, volumeSlider ); } );
		sfxSlider.onValueChanged.AddListener( delegate {TriggerSliderUpdate( GameDataModel.GameSettings.sound, sfxSlider ); } );
		firewallSlider.onValueChanged.AddListener( delegate {TriggerSliderUpdate( GameDataModel.GameSettings.firewall, firewallSlider ); } );


		EventTrigger.Entry reset_selected_entry = new EventTrigger.Entry();
		reset_selected_entry.eventID = EventTriggerType.PointerDown;
		reset_selected_entry.callback.AddListener( (eventData) => { TriggerResetSelected(); } );
		resetEventTrigger.triggers.Add(reset_selected_entry);

		EventTrigger.Entry reset_deselected_entry = new EventTrigger.Entry();
		reset_deselected_entry.eventID = EventTriggerType.PointerUp;
		reset_deselected_entry.callback.AddListener( (eventData) => { TriggerResetDeSelected(); } );
		resetEventTrigger.triggers.Add(reset_deselected_entry);
	}

	void Update()
	{
		if( uiActive )
		{
			if(_firewallOff)
			{
				float timePerlin = Mathf.PerlinNoise(Time.unscaledTime, Time.unscaledTime);
				firewall_IMG.sprite = (timePerlin > 0.6f) ? firewall_off_alt : firewall_off;
			}	
			if(_resetPressed)
			{
				_resetTimer += Time.unscaledDeltaTime * 0.5f;
				_resetTimer = Mathf.Clamp(_resetTimer, 0f, 1f);

				if(_resetTimer == 1f)
				{
					GameManager.instance.ReportClearGameData();
					_resetTimer = 0f;
					_resetPressed = false;
					LoadSettingValues();
				}

				resetSlider.value = _resetTimer / 1f;
			}
			else if(_resetTimer != 0f)
			{
				_resetTimer -= Time.unscaledDeltaTime;
				_resetTimer = Mathf.Clamp(_resetTimer, 0f, 1f);
				resetSlider.value = _resetTimer / 1f;
			}
		}
	}

	void TriggerSliderUpdate( GameDataModel.GameSettings option, Slider slider )
	{
		Debug.Log( option.ToString());	
		PlayerPrefs.SetFloat( option.ToString(), slider.value );
		switch(option)
		{
			case GameDataModel.GameSettings.firewall:
				int firewall_index = (int) slider.value ;
				_firewallOff = (firewall_index==0);
				firewall_IMG.sprite = (firewall_index==1) ? firewall_on : firewall_off;
				break;
		}
	}

	void TriggerResetSelected()
	{
		_resetPressed = true;
	}
	void TriggerResetDeSelected()
	{
		_resetPressed = false;	
	}
}
