using UnityEngine;
using System.Collections;

public class MenuSelection : MonoBehaviour {
	
	public Camera cameraObject; //Add the camera object to this slot in the Inspector
	public GameObject scriptObject; //Add the singleton script game object to this slot in the Inspector

	GameObject collisionObject;
	bool moveCamera;
	float speed = 1;

	private InputDetector inputDetector;
	
	// Use this for initialization
	void Start () {
		if (scriptObject != null) {
			inputDetector = (InputDetector)scriptObject.GetComponent ("InputDetector");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (inputDetector.touchpadIsTapped()) {
			collisionObject = raycastForObject ();
			GameObject.Find ("DebugText").GetComponent<TextMesh> ().text = collisionObject.name;
			//distance = hitInfo.distance;
			if(collisionObject.name.StartsWith("Planet")){
				
				//float step = speed * Time.deltaTime;
				//GameObject.Find("Navigation").transform.position = Vector3.MoveTowards(GameObject.Find("Navigation").transform.position, hitInfo.collider.transform.position, step);
				moveCamera = true;
				Debug.Log("Hit Planet");
			}
		} else {
			//distance = maxCrosshairDist;
		}

		if(moveCamera){
			float step = speed * Time.deltaTime;
			GameObject.Find("Navigation").transform.position = Vector3.MoveTowards(GameObject.Find("Navigation").transform.position, collisionObject.transform.position, step);
			
			if((GameObject.Find("Navigation").transform.position-collisionObject.transform.position).sqrMagnitude<=1){
				moveCamera = false;
			}

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
