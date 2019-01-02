using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WinVectorData
{
	public bool isValid;
	public Vector2 start;
	public Vector2 end;
	public WinVectorData()
	{
		this.start = Vector2.zero;
		this.end = Vector2.zero;
		this.isValid = false;
	}
	public WinVectorData(Vector2 inputStart, Vector2 inputEnd)
	{
		this.start = inputStart;
		this.end = inputEnd;
		this.isValid = true;
	}
}