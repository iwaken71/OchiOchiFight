using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class CubeModel : MonoBehaviour
{

	Vector3 _startPosition;
	//Rigidbody _rigidbody;

	public StateReactiveProperty cubeState;

	Material _material;
	Color defaultColor;


	void Start ()
	{
		_startPosition = transform.position;
		_material = GetComponent<Renderer> ().material;
		defaultColor = _material.color;

		//_rigidbody = GetComponent<Rigidbody> ();
		cubeState = new StateReactiveProperty (CubeState.Steady);
		cubeState.Where (s => s == CubeState.NoGame)
		.Subscribe (_ => {
			OnNoGameState ();
		});
		cubeState.Where (s => s == CubeState.Steady)
		.Subscribe (_ => {
			OnSteadyState ();
		});
		cubeState.Where (s => s == CubeState.Changed)
		.Subscribe (_ => {
			OnChangedState ();
		});
		cubeState.Where (s => s == CubeState.Falling)
		.Subscribe (_ => {
			OnFallingState ();
		});
		cubeState.Where (s => s == CubeState.DeadChanged)
		.Subscribe (_ => {
			OnDeadChangedState ();
		});
		cubeState.Where (s => s == CubeState.DeadFalling)
		.Subscribe (_ => {
			OnDeadFallingState ();
		});


	}

	public void SelectState (CubeState input)
	{
		cubeState.Value = input;
	}

	public void FallingMode (Color input)
	{
		if (IsDead ())
			return;
		cubeState.Value = CubeState.Changed;
		float r = input.r;
		float b = input.b;
		float g = input.g;
		float change = 0.25f;
		r += change;
		if (r >= 1)
			r = 1;

		b += change;
		if (b >= 1)
			b = 1;
		
		g += change;
		if (g >= 1)
			g = 1;
		_material.color = new Color (r, g, b);
	}

	IEnumerator SetState (CubeState input, float timer = 0f)
	{
		yield return new WaitForSeconds (timer);

		if (input == CubeState.Steady) {
			if (IsDead ()) {
				yield break;
			}
		}
		cubeState.Value = input;
	}

	void OnNoGameState ()
	{
		ResetCube ();
		_material.color = defaultColor;
	}

	void OnSteadyState ()
	{
		ResetCube ();
		_material.color = defaultColor;
	}

	void OnChangedState ()
	{
		StartCoroutine (SetState (CubeState.Falling, 0.7f));
	}

	void OnFallingState ()
	{
		StartCoroutine (SetState (CubeState.Steady, 4.0f));
		AudioManager.Instance.PlaySE (Const.Audio.fall);
		//_rigidbody.velocity = Vector3.down * 5;
	}

	void OnDeadChangedState ()
	{
		_material.color = Color.gray;
		StartCoroutine (SetState (CubeState.DeadFalling, 1.5f));
	}

	void OnDeadFallingState ()
	{
		AudioManager.Instance.PlaySE (Const.Audio.fall);
		_material.color = Color.black;
	}

	void ResetCube ()
	{
		transform.position = _startPosition;
		gameObject.SetActive (true);
	}



	public bool IsSteady {
		get {
			return cubeState.Value == CubeState.Steady;
		}
	}

	void Update ()
	{
		if (IsFalling ()) {
			transform.position += Vector3.down * 5 * Time.deltaTime;

		}
	}

	bool IsDead ()
	{
		return  cubeState.Value == CubeState.DeadChanged || cubeState.Value == CubeState.DeadFalling;
	}

	bool IsFalling ()
	{
		return  cubeState.Value == CubeState.Falling || cubeState.Value == CubeState.DeadFalling;
	}

}

public enum CubeState
{
	NoGame = 0,
	Steady = 1,
	Changed = 2,
	Falling = 3,
	DeadChanged = 4,
	DeadFalling = 5
}

[Serializable]
public class StateReactiveProperty : ReactiveProperty<CubeState>
{
	public StateReactiveProperty ()
	{
	}

	public StateReactiveProperty (CubeState initialValue)
		: base (initialValue)
	{
	}
}

