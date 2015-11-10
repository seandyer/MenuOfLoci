using UnityEngine;
using System.Collections;

public class MenuSelection : MonoBehaviour {
	
	public Camera cameraObject; //Add the camera object to this slot in the Inspector
	public GameObject scriptObject; //Add the singleton script game object to this slot in the Inspector
	public MovieTexture movie;
	private InputDetector inputDetector;
	private bool moving = false;
	private float speed = 8.0f;
	private GameObject target;
	// Use this for initialization
	void Start () {

		if (scriptObject != null) {
			inputDetector = (InputDetector)scriptObject.GetComponent ("InputDetector");
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (inputDetector.touchpadIsTapped()) {
			target = raycastForObject ();
			GameObject.Find ("DebugText").GetComponent<TextMesh> ().text = target.name;
			movie = target.GetComponent<Renderer>().material.mainTexture as MovieTexture;


			movie.Play();
			if (target != null && target.tag.Equals("Category")) {
				moving = true;	
			}
		}
		if (moving) {
			checkForMove (target);	
		}
	}
	private void checkForMove(GameObject destination) {
		GameObject user = GameObject.Find ("Main Camera");
		var heading = destination.transform.position - user.transform.position;
		var distance = heading.magnitude;
		if (distance <= 1.0f) {
			moving = false;
		} else {
			heading = heading.normalized;
			float move = speed * Time.deltaTime;
			if (move > distance) {
				move = distance;
			}
			user.transform.Translate (heading * move);


		}


	}
	private GameObject raycastForObject() {
		RaycastHit hitInfo;
		
		//get head position and gaze direction
		Vector3 headPosition = cameraObject.transform.position;
		Vector3 gazeDirection = cameraObject.transform.forward;
		
		if (Physics.Raycast (headPosition, gazeDirection, out hitInfo)) {
			GameObject raycastObject = hitInfo.transform.gameObject;
			
			return raycastObject;
		}
		return null;
	}
}
