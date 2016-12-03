using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	[SerializeField]
	int playerNumber = 2;
	public static GameManager instance = null;

	float timer = 0;

	public GameStateReactiveProperty gameState;

	[SerializeField]
	float finishTimeFirst = 90;

	float finishTime = 90;

	[HideInInspector]
	public IntReactiveProperty NumberOfPlayer;

	GameObject[,] cubes = new GameObject[8, 8];


	GameObject stageParentObject;
	GameObject cubePrefab1, cubePrefab2;


	List<GameObject> playerList;
	public List<IPlayer> playerInfoList;




	void Awake ()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
			finishTime = finishTimeFirst;
			init ();
		} else {
			Destroy (this.gameObject);
		}
	}

	public float GameTimer {
		get { 
			if (timer < 0) {
				return 0;
			}
			return timer; 
		}
	}

	public int RestTimer {
		get { 
			float restTime = finishTime - timer;
			int restIntTime = (int)restTime;
			if (restIntTime >= finishTime) {
				restTime = finishTime;
			}
			return restIntTime;

		}
	}

	public int NumberOfPlayers{ get { return NumberOfPlayer.Value; } }

	void init ()
	{
		gameState = new GameStateReactiveProperty (GameState.PreGame);
		NumberOfPlayer = new IntReactiveProperty (playerNumber);
		playerList = new List<GameObject> ();
		playerInfoList = new List<IPlayer> ();

		//timer = 0;
		//GenerateStage ();
		//GeneratePlayer (2);


	}

	void Start ()
	{
		gameState.Where (s => s == GameState.Start)
		.Subscribe (_ => {
			OnStartStateEnter ();
		});
		gameState.Where (s => s == GameState.PreGame)
		.Subscribe (_ => {
			OnPreGameStateEnter ();
		});
		gameState.Where (s => s == GameState.Game)
		.Subscribe (_ => {
			OnGameStateEnter ();
		});
		gameState.Where (s => s == GameState.Result)
		.Subscribe (_ => {
			OnResultStateEnter ();
		});

		gameState.Where (s => s == GameState.PreGame)
		.Delay (TimeSpan.FromSeconds (3))
		.Subscribe (_ => {
			gameState.Value = GameState.Game;
		});

		gameState.Where (s => s == GameState.Game)
		.Delay (TimeSpan.FromSeconds (finishTime / 2))
		.Subscribe (_ => {
			StartCoroutine (ProcessSmallState ());

		});
		gameState.Where (s => s == GameState.Game)
		.Delay (TimeSpan.FromSeconds (finishTime * 3 / 4))
		.Subscribe (_ => {
			StartCoroutine (ProcessSmallState ());
		});

	}

	void OnStartStateEnter ()
	{

	}

	void OnPreGameStateEnter ()
	{
		StartCoroutine (SoundCountDown ());
		timer = 0;
		GenerateStage ();
		GeneratePlayer (playerNumber);
		GameUI.instance.SetPlayerInfo ();

	}

	void  OnGameStateEnter ()
	{
		
	}

	void OnResultStateEnter ()
	{
		AudioManager.Instance.StopBGM ();
	}

	void Update ()
	{
		if (gameState.Value == GameState.Game) {
			timer += Time.deltaTime;
			if (timer > finishTime) {
				gameState.Value = GameState.Result;
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			GameStart ();
		}


	}

	void GameStart ()
	{
		timer = 0;
		GenerateStage ();
		GeneratePlayer (playerNumber);


		gameState.Value = GameState.PreGame;

	}

	IEnumerator SoundCountDown ()
	{
		AudioManager.Instance.PlaySE (Const.Audio.countDown);
		yield return new WaitForSeconds (1f);
		AudioManager.Instance.PlaySE (Const.Audio.countDown);
		yield return new WaitForSeconds (1f);
		AudioManager.Instance.PlaySE (Const.Audio.countDown);
		yield return new WaitForSeconds (1f);
		AudioManager.Instance.PlaySE (Const.Audio.startSound);
		yield return new WaitForSeconds (0.1f);
		AudioManager.Instance.PlayBGM (Const.Audio.kb2);

	}

	void GenerateStage ()
	{
		if (stageParentObject == null) {
			stageParentObject = new GameObject ("Stage");
		} else {
			DeleteStage ();
			stageParentObject = new GameObject ("Stage");
		}
		cubePrefab1 = Resources.Load ("Cube1")as GameObject;
		cubePrefab2 = Resources.Load ("Cube2")as GameObject;

		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				if ((i + j) % 2 == 0) {
					cubes [i, j] = Instantiate (cubePrefab1, new Vector3 (i, 0, j), Quaternion.identity)as GameObject;
				} else {
					cubes [i, j] = Instantiate (cubePrefab2, new Vector3 (i, 0, j), Quaternion.identity)as GameObject;
				}
				cubes [i, j].transform.parent = stageParentObject.transform;
			}
		}
	}

	void DeleteStage ()
	{
		Destroy (stageParentObject);
	}

	void GeneratePlayer (int number)
	{
		DeleteAllPlayer ();
		if (number <= 0)
			return;
		if (number > 4)
			number = 4;

		NumberOfPlayer.Value = number;

		for (int i = 0; i < number; i++) {
			if (i == 0) {

				GameObject obj = Instantiate (Resources.Load <GameObject> ("Player1"));
				obj.transform.position = cubes [1, 1].transform.position + Vector3.up * (0.75f - cubes [1, 1].transform.position.y);
				obj.transform.forward = Vector3.right;
				playerList.Add (obj);
				IPlayer p = obj.GetComponent<Player> ();
				p.ResetInfo ();
				playerInfoList.Add (p);


			} else if (i == 1) {
				GameObject obj = Instantiate (Resources.Load <GameObject> ("Player2"));
				obj.transform.position = cubes [6, 6].transform.position + Vector3.up * (0.75f - cubes [6, 6].transform.position.y);
				obj.transform.forward = Vector3.left;
				playerList.Add (obj);
				IPlayer p = obj.GetComponent<Player> ();
				p.ResetInfo ();
				playerInfoList.Add (p);

			} else if (i == 2) {
				GameObject obj = Instantiate (Resources.Load <GameObject> ("Player3"));
				obj.transform.position = cubes [6, 1].transform.position + Vector3.up * (0.75f - cubes [6, 1].transform.position.y);
				obj.transform.forward = Vector3.forward;
				playerList.Add (obj);
				IPlayer p = obj.GetComponent<Player> ();
				p.ResetInfo ();
				playerInfoList.Add (p);
			} else if (i == 3) {
				GameObject obj = Instantiate (Resources.Load <GameObject> ("Player4"));
				obj.transform.position = cubes [1, 6].transform.position + Vector3.up * (0.75f - cubes [1, 6].transform.position.y);
				obj.transform.forward = Vector3.back;
				playerList.Add (obj);
				IPlayer p = obj.GetComponent<Player> ();
				p.ResetInfo ();
				playerInfoList.Add (p);

			}

			NumberOfPlayer.Value = number;
		}
	}

	void DeleteAllPlayer ()
	{
		foreach (GameObject player in playerList) {
			
			Destroy (player);
		}
		playerList = new List<GameObject> ();
		playerInfoList = new List<IPlayer> ();
	
	}

	// ステージを小さくする (周りのブロックを落とす)
	IEnumerator ProcessSmallState ()
	{
		int number = 1;
		while (true) {
			if (cubes [number - 1, number - 1].GetComponent<CubeModel> ().cubeState.Value == CubeState.DeadFalling) {
				number++;
				if (number >= 3) {
					yield break;
				}
			} else {
				break;
			}
		}
		int firstIndex = number - 1;
		int lastIndex = cubes.GetLength (0) - number;

		float delay = 0.1f;

		for (int i = 0; i <= lastIndex; i++) {
			cubes [i, firstIndex].GetComponent<CubeModel> ().SelectState (CubeState.DeadChanged);
			yield return new WaitForSeconds (delay);
			cubes [i, lastIndex].GetComponent<CubeModel> ().SelectState (CubeState.DeadChanged);
			yield return new WaitForSeconds (delay);
		}

		for (int j = number; j <= lastIndex - 1; j++) {
			cubes [firstIndex, j].GetComponent<CubeModel> ().SelectState (CubeState.DeadChanged);
			yield return new WaitForSeconds (delay);
			cubes [lastIndex, j].GetComponent<CubeModel> ().SelectState (CubeState.DeadChanged);
			yield return new WaitForSeconds (delay);
		}

		

	}


}

public enum GameState
{
	Start = 0,
	PreGame = 1,
	Game = 2,
	Result = 3,

}

[Serializable]
public class GameStateReactiveProperty : ReactiveProperty<GameState>
{
	public GameStateReactiveProperty ()
	{
	}

	public GameStateReactiveProperty (GameState initialValue)
		: base (initialValue)
	{
	}
}