using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataController : MonoBehaviour
{

	public enum PlayerDataTypes {p1_name, p2_name, p1_score, p2_score}


	public void SetPlayerDataValue( PlayerDataTypes dataType, string value)
	{
		Debug.Log("Setting data type " + dataType.ToString() + " to " + value);
		PlayerPrefs.SetString( dataType.ToString(), value );
	}
	public void SetPlayerDataValue( PlayerDataTypes dataType, int value)
	{
		PlayerPrefs.SetInt( dataType.ToString(), value );
	}

}
