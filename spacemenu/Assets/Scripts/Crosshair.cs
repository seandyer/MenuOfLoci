using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Camera cameraObject;
	public float maxCrosshairDist = 100;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hitInfo;
		float distance;

		if (Physics.Raycast (new Ray (cameraObject.transform.position, cameraObject.transform.rotation * Vector3.forward), out hitInfo)) {
			distance = Mathf.Min(hitInfo.distance, maxCrosshairDist);
		} else {
			distance = maxCrosshairDist;
		}
		transform.position = cameraObject.transform.position + cameraObject.transform.rotation * Vector3.forward * distance;
		transform.LookAt (cameraObject.transform.position);
		transform.Rotate (0.0f, 180.0f, 0.0f);
	}
}
