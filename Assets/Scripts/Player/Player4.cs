using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player4 : Player
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
		float h = Input.GetAxis ("Horizontal4");
		float v = Input.GetAxis ("Vertical4");
		Input_h = h;
		Input_v = v;
		IsPushedShotButton = Input.GetKeyDown (KeyCode.Joystick4Button16) || Input.GetKeyDown (KeyCode.Joystick4Button0);
		base.Update ();
	}
}


