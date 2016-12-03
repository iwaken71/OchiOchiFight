using UnityEngine;
using System.Collections;
using System;
using UniRx;

public interface IPlayer
{
	PlayerInfo GetPlayerInfo ();

	void ResetInfo ();

	//IntReactiveProperty FallCount ();
	int FallCount ();

}

public enum PlayerState
{
	Move = 0,
	PreFall = 1,
	Fall = 2,
	Stop = 3,

}

[Serializable]
public class PlayerStateReactiveProperty : ReactiveProperty<PlayerState>
{
	public PlayerStateReactiveProperty ()
	{
	}

	public PlayerStateReactiveProperty (PlayerState initialValue)
		: base (initialValue)
	{
	}
}