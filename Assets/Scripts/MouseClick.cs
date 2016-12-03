using UnityEngine;
using System.Collections;

public class MouseClick : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 1000)) {
				if (hit.collider.tag == "Cube") {
					CubeModel m = hit.collider.GetComponent<CubeModel> ();
					//m.FallingMode ();
				}
			}
		}
	
	}
}
