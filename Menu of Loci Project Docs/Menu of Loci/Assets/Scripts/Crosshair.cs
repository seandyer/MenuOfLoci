using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Camera cameraObject;
	public float maxCrosshairDist = 100;
	float speed = 1;
	bool moveCamera;
	GameObject collisionObject;

	// Use this for initialization
	void Start () {
	
	}

	/*
	// Update is called once per frame
	void Update () {
		RaycastHit hitInfo;
		float distance;

		if (Physics.Raycast (new Ray (cameraObject.transform.position, cameraObject.transform.rotation * Vector3.forward), out hitInfo)) {
			distance = hitInfo.distance;
		} else {
			distance = maxCrosshairDist;
			//distance = cameraObject.farClipPlane * 0.95f;
		}
		//transform.position = cameraObject.transform.position + cameraObject.transform.rotation * Vector3.forward * distance;
		//transform.LookAt (cameraObject.transform.position);
		//transform.Rotate (0.0f, 180.0f, 0.0f);

	}
	*/
	void Update () {
		RaycastHit hitInfo;
		float distance;
		
		if (Physics.Raycast (new Ray (cameraObject.transform.position, cameraObject.transform.rotation * Vector3.forward), out hitInfo)) {
			distance = hitInfo.distance;
			if(hitInfo.collider.name == "planet"){
				
				float step = speed * Time.deltaTime;
				GameObject.Find("Navigation").transform.position = Vector3.MoveTowards(GameObject.Find("Navigation").transform.position, hitInfo.collider.transform.position, step);
				collisionObject = hitInfo.collider.gameObject;
				moveCamera = true;
				Debug.Log("Hit Planet");
			}
		} else {
			distance = maxCrosshairDist;
			//distance = cameraObject.farClipPlane * 0.95f;
		}
		
		if(moveCamera){
			float step = speed * Time.deltaTime;
			GameObject.Find("Navigation").transform.position = Vector3.MoveTowards(GameObject.Find("Navigation").transform.position, hitInfo.collider.transform.position, step);
			
			if(GameObject.Find("Navigation").transform.position == collisionObject.transform.position){
				moveCamera = false;
			}
			
		}
		transform.position = cameraObject.transform.position + cameraObject.transform.rotation * Vector3.forward * distance;
		transform.LookAt (cameraObject.transform.position);
		transform.Rotate (0.0f, 180.0f, 0.0f);
		
	}
}
