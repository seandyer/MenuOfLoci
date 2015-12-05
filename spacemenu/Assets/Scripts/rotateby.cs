using UnityEngine;
using System.Collections;

public class rotateby : MonoBehaviour {
	public float X=0.0f,Y=0.1f,Z=0.0f;

	
	// Update is called once per frame
	void Update () {

		transform.Rotate(new Vector3(X,Y,Z));
	
	}
}
