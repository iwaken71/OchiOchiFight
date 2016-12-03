using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class PlayerPanel : MonoBehaviour
{
	[SerializeField]
	Text infoLabel;

	[SerializeField]
	int number;

	IPlayer info;

	void Awake ()
	{

	}

	void Start ()
	{
		/*
		if (GameManager.instance.playerInfoList.Count > 0) {
			//info = null;
			info = GameManager.instance.playerInfoList [number];
			//Debug.Log (GameManager.instance.playerInfoList);
		}
		//info = GameManager.instance.playerInfoList [number];
		//Debug.Log (1);
		Debug.Log (number);



		GameManager.instance.NumberOfPlayer.Subscribe (_ => {
			Debug.Log (GameManager.instance.playerInfoList.Count);
			if (GameManager.instance.playerInfoList.Count > 0) {
				info = null;
				info = GameManager.instance.playerInfoList [number];
			}
		});
		/*
		info.fallCount.Subscribe (x => {
			infoLabel.text = "落ちた回数: " + x.ToString ();
		});*/


		/*
		GameManager.instance.gameState
		.Where (s => s == GameState.PreGame)
		.Subscribe (_ => {
			//Debug.Log (1);
			//	info = null;

		
			//Debug.Log (info.GetPlayerInfo ().FallCount);
		});
		GameManager.instance.gameState
		.Where (s => s == GameState.Game)
		.Subscribe (_ => {
			//	info = null;
			//Debug.Log (2);
			info = GameManager.instance.playerInfoList [number];

			//Debug.Log (info.GetPlayerInfo ().FallCount);
		});
		*/
	}

	public void SetInfo (IPlayer input)
	{
		info = input;
	}

	
	// Update is called once per frame
	void Update ()
	{

		
		if (info != null) {
			
			infoLabel.text = "落ちた回数: " + info.FallCount ().ToString ();
			//Debug.Log (gameObject.name + ":" + info.FallCount ().ToString ());
		
		} else {
			if (GameManager.instance.playerInfoList.Count > 0) {
				//info = GameManager.instance.playerInfoList [number];
				/*
				info.fallCount.Subscribe (x => {
					infoLabel.text = "落ちた回数: " + x.ToString ();
				});	*/
			}
		}
		
	
	}

	bool IsGameState ()
	{
		return GameManager.instance.gameState.Value == GameState.Game;
	}

	bool IsPreGameState ()
	{
		return GameManager.instance.gameState.Value == GameState.PreGame;
	}
}
