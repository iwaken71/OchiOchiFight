using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray ray = new Ray (transform.position, Vector3.down);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 1)) {
			if (hit.collider.tag == "Cube") {
				//hit.collider.GetComponent<CubeModel> ().FallingMode ();
			}
		}
	
	}
}
