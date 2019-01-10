using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SETTINGS_UIController : UIController
{

	public Slider volumeSlider, sfxSlider, firewallSlider;

	public Image firewall_IMG;
	public Sprite firewall_off, firewall_off_alt, firewall_on;
	bool firewall_is_off;

	public override void OpenUI ()
	{
		ActivateSliderListeners();
		LoadSettingValues();
		base.OpenUI ();
	}

	void LoadSettingValues()
	{
		volumeSlider.value = PlayerPrefs.GetFloat( GameDataModel.GameSettings.volume.ToString(), 0f );
		sfxSlider.value = PlayerPrefs.GetFloat( GameDataModel.GameSettings.sound.ToString(), 0f );
		firewallSlider.value = PlayerPrefs.GetFloat( GameDataModel.GameSettings.firewall.ToString(), 1f );
	}

	void ActivateSliderListeners()
	{
		volumeSlider.onValueChanged.RemoveAllListeners();
		sfxSlider.onValueChanged.RemoveAllListeners();
		firewallSlider.onValueChanged.RemoveAllListeners();

		volumeSlider.onValueChanged.AddListener( delegate { TriggerSliderUpdate( GameDataModel.GameSettings.volume, volumeSlider ); } );
		sfxSlider.onValueChanged.AddListener( delegate {TriggerSliderUpdate( GameDataModel.GameSettings.sound, sfxSlider ); } );
		firewallSlider.onValueChanged.AddListener( delegate {TriggerSliderUpdate( GameDataModel.GameSettings.firewall, firewallSlider ); } );
	}

	void Update()
	{
		if( uiActive )
		{
			if(firewall_is_off)
			{
				float timePerlin = Mathf.PerlinNoise(Time.unscaledTime, Time.unscaledTime);
				firewall_IMG.sprite = (timePerlin > 0.6f) ? firewall_off_alt : firewall_off;
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
				firewall_is_off = (firewall_index==0);
				firewall_IMG.sprite = (firewall_index==1) ? firewall_on : firewall_off;
				break;
		}
	}
}
