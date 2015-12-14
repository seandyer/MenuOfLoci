using UnityEngine;
using System.Collections;

public class revolve : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {

		transform.RotateAround(Vector3.zero,Vector3.up,0.1f);
	
	}
}
