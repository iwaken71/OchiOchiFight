using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour,IPlayer
{

	public PlayerStateReactiveProperty state;

	protected float speed = 3;
	public IntReactiveProperty fallCount = new IntReactiveProperty (0);

	protected bool isGround = true;

	protected Vector3 _startPosition;
	protected Color myColor;

	private float input_h = 0, input_v = 0;
	private bool isPushedShotButton = false;

	private PlayerInfo playerInfo;

	public PlayerInfo GetPlayerInfo ()
	{
		return playerInfo;
	}
	// Use this for initialization

	protected void Awake ()
	{
		state = new PlayerStateReactiveProperty (PlayerState.Move);
		myColor = GetComponent<Renderer> ().material.color;
		playerInfo = new PlayerInfo ();
		fallCount.Value = 0;
	}

	public int FallCount ()
	{
		
		return fallCount.Value;
	}

	protected void Start ()
	{
		


		state.Where (s => s == PlayerState.Move)
		.Subscribe (_ => {
			
		});

		state.Where (s => s == PlayerState.PreFall)
		.Subscribe (_ => {
			
			if (GameManager.instance.gameState.Value == GameState.Game) {
				playerInfo.FallCount++;
				fallCount.Value++;
				AudioManager.Instance.PlaySE (Const.Audio.fallPlayer);
				//Debug.Log (fallCount.Value);
			}

		});
		state.Where (s => s == PlayerState.Fall)
		.Delay (TimeSpan.FromSeconds (3.0f))
		.Subscribe (_ => {
			state.Value = PlayerState.Move;
			transform.position = RandomStartPoint ();
			
		});

		state.Where (s => s == PlayerState.Stop)
		.Delay (TimeSpan.FromSeconds (0.7f))
		.Subscribe (_ => {
			state.Value = PlayerState.Move;
		});
		state.Where (s => s == PlayerState.PreFall)
		.Delay (TimeSpan.FromSeconds (0.01f))
		.Subscribe (_ => {
			state.Value = PlayerState.Fall;
		});

		fallCount.Subscribe (x => {
			playerInfo.FallCount = x;
		});


	
	}

	protected float Input_h {
		set{ input_h = value; }
		get{ return input_h; }
	}

	protected float Input_v {
		set{ input_v = value; }
		get{ return input_v; }
	}

	protected bool IsPushedShotButton {
		set{ isPushedShotButton = value; }
	}

	public void ResetInfo ()
	{
		playerInfo.ResetInfo ();
	}


	// Update is called once per frame
	protected void Update ()
	{
		if (state.Value == PlayerState.Move) {
			float h = input_h;
			float v = input_v;
			float limit = 0.5f;
			if (Mathf.Abs (h) < limit)
				h = 0;
			if (Mathf.Abs (v) < limit)
				v = 0;

			if (Mathf.Abs (h) > Mathf.Abs (v)) {
				transform.forward = (Vector3.right * h).normalized;
				transform.position += new Vector3 (h, 0, 0) * Time.deltaTime * speed;
			} else if (Mathf.Abs (h) < Mathf.Abs (v)) {
				transform.forward = (Vector3.forward * v).normalized;
				transform.position += new Vector3 (0, 0, v) * Time.deltaTime * speed;
			}
	
			if (isPushedShotButton && GameManager.instance.gameState.Value == GameState.Game) {
				AudioManager.Instance.PlaySE (Const.Audio.swish);
				for (int i = 1; i <= 8; i++) {
					Vector3 point = transform.position + transform.forward * i;

					Ray ray = new Ray (point, Vector3.down);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, 5)) {
						if (hit.collider.tag == "Cube") {
							StartCoroutine (FallCube (hit.collider.GetComponent<CubeModel> (), 0.15f * i));
						}
					}
				}
				state.Value = PlayerState.Stop;
			}

		
			FixPosition ();
			CheckOnCube ();
		
		} else if (state.Value == PlayerState.PreFall) {
			// 落ち始める処理
			transform.position += Vector3.down * Time.deltaTime * 0.1f;
		} else if (state.Value == PlayerState.Fall) {
			// 落ち続ける処理
			transform.position += Vector3.down * Time.deltaTime * 5;
		
		}
	}


	void FixPosition ()
	{
		Vector3 pos = transform.position;
		float x = pos.x, z = pos.z;
		if (pos.x < -0.5f) {
			x = -0.5f;
		} else if (pos.x > 7.5f) {
			x = 7.5f;
		}

		if (pos.z < -0.5f) {
			z = -0.5f;
		} else if (pos.z > 7.5f) {
			z = 7.5f;
		}

		transform.position = new Vector3 (x, pos.y, z);

	}

	void CheckOnCube ()
	{

		Ray rayGround = new Ray (transform.position, Vector3.down);
		RaycastHit hitInfo;
		if (Physics.Raycast (rayGround, out hitInfo, 2.0f)) {
			if (hitInfo.collider.tag == "Floor") {
				state.Value = PlayerState.PreFall;
			}
		} 
	}





	protected IEnumerator FallCube (CubeModel c, float time)
	{
		yield return new WaitForSeconds (time);
		c.FallingMode (myColor);
	}




	protected Vector3 RandomStartPoint ()
	{
		GameObject[] cubes = GameObject.FindGameObjectsWithTag ("Cube");
		List<GameObject> cubeList = new List<GameObject> ();
		foreach (GameObject o in cubes) {
			cubeList.Add (o);
		}
		List<GameObject> newList = 
			cubeList
			.Where (g => g.GetComponent<CubeModel> ().cubeState.Value == CubeState.Steady)
			.ToList ();
		int number = newList.Count;
		if (number <= 0)
			return Vector3.zero;

		int random = UnityEngine.Random.Range (0, number);
		GameObject selectedCube = newList [random];

		return selectedCube.transform.position + Vector3.up * (0.75f - selectedCube.transform.position.y);
	}
}
