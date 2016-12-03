using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player2 : Player
{
	void Awake ()
	{
		base.Awake ();
	}

	void Start ()
	{
		base.Start ();
	}


	void Update ()
	{
		float h = Input.GetAxis ("Horizontal2");
		float v = Input.GetAxis ("Vertical2");
		Input_h = h;
		Input_v = v;

		IsPushedShotButton = Input.GetKeyDown (KeyCode.Joystick2Button16) || Input.GetKeyDown (KeyCode.RightShift) || Input.GetKeyDown (KeyCode.Joystick2Button0);
		base.Update ();
	}
}


