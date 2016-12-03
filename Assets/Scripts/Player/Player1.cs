using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player1 : Player
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
		float h = Input.GetAxis ("Horizontal1");
		float v = Input.GetAxis ("Vertical1");
		Input_h = h;
		Input_v = v;
		IsPushedShotButton = Input.GetKeyDown (KeyCode.Joystick1Button16) || Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Joystick1Button0);
		base.Update ();
	}
}


