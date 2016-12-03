using UnityEngine;
using System.Collections;

public class PlayerInfo
{
	int fallCount;

	public int FallCount {
		set{ fallCount = value; }
		get{ return fallCount; }
	}

	public PlayerInfo ()
	{
		fallCount = 0;
	}

	public void ResetInfo ()
	{
		fallCount = 0;
	}
}
