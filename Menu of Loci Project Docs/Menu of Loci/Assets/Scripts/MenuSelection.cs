using UnityEngine;
using System.Collections;

public class MenuSelection : MonoBehaviour {

	public GameObject userObject; //Add the user/navigation object to this slot in the Inspector
	public Camera cameraObject; //Add the camera object to this slot in the Inspector
	public GameObject scriptObject; //Add the singleton script game object to this slot in the Inspector
	public float speed = 2.5f;

	GameObject collisionObject;
	bool moveCamera;

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
			if(collisionObject.tag.Equals("Category")){
				
				//float step = speed * Time.deltaTime;
				//GameObject.Find("Navigation").transform.position = Vector3.MoveTowards(GameObject.Find("Navigation").transform.position, hitInfo.collider.transform.position, step);
				moveCamera = true;
				Debug.Log("Hit Planet");
			} else if (collisionObject.tag.Equals("Video")){
				Handheld.PlayFullScreenMovie("Uncharted.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
			}

		} else {
			//distance = maxCrosshairDist;
		}

		if(moveCamera){
			float step = speed * Time.deltaTime;
			userObject.transform.position = Vector3.MoveTowards(userObject.transform.position, collisionObject.transform.position, step);
			
			if((userObject.transform.position-collisionObject.transform.position).sqrMagnitude <= 1){
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
