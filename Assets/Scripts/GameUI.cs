using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class GameUI : MonoBehaviour
{

	[SerializeField]
	Text timerText;

	[SerializeField]
	GameObject[] playerPanel;

	public static GameUI instance = null;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
			init ();
		}

	}

	public void init ()
	{
		int number = GameManager.instance.NumberOfPlayers;
		for (int i = 0; i < playerPanel.Length; i++) {
			
			if (i < number) {
				playerPanel [i].SetActive (true);
			} else {
				playerPanel [i].SetActive (false);
			}
		}
		GameManager.instance.gameState
		.Where (s => s == GameState.PreGame)
		.Subscribe (_ => {
			
			timerText.text = GameManager.instance.RestTimer.ToString () + "s";
			for (int i = 0; i < playerPanel.Length; i++) {
				//int number = GameManager.instance.NumberOfPlayers;
				if (i < number) {
					playerPanel [i].SetActive (true);
				} else {
					playerPanel [i].SetActive (false);
				}
			}
		});
	}

	// Use this for initialization
	void Start ()
	{
		



	}

	public void SetPlayerInfo ()
	{
		int number = GameManager.instance.NumberOfPlayers;
		for (int i = 0; i < 4; i++) {
			if (i < number) {
				playerPanel [i].GetComponent<PlayerPanel> ().SetInfo (GameManager.instance.playerInfoList [i]);
	
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (IsGameState () || IsPreGameState ()) {
			timerText.text = GameManager.instance.RestTimer.ToString () + "s";
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
