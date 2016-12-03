using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player3 : Player
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
		float h = Input.GetAxis ("Horizontal3");
		float v = Input.GetAxis ("Vertical3");
		Input_h = h;
		Input_v = v;
		IsPushedShotButton = Input.GetKeyDown (KeyCode.Joystick3Button16) || Input.GetKeyDown (KeyCode.Joystick3Button0);
		base.Update ();
	}
}


