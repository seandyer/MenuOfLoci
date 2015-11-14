using UnityEngine;
using System.Collections;

public class MenuSelection : MonoBehaviour {

	public GameObject userObject; //Add the user/navigation object to this slot in the Inspector
	public Camera cameraObject; //Add the camera object to this slot in the Inspector
	public GameObject scriptObject; //Add the singleton script game object to this slot in the Inspector
	public float speed = 10f;

	private GameObject collisionObject;
	private bool moveCamera;
	private float destinationOffset;
	private Vector3 destinationPosition;
	private Vector3 startingPosition;

	private InputDetector inputDetector;
	
	// Use this for initialization
	void Start () {
		if (scriptObject != null) {
			inputDetector = (InputDetector)scriptObject.GetComponent ("InputDetector");
		}
		userObject = GameObject.Find ("Navigation");
		if (userObject != null) {
			startingPosition = userObject.transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//User has tapped the touchpad
		if (inputDetector.touchpadIsTapped()) {
			collisionObject = raycastForObject ();
			GameObject.Find ("DebugText").GetComponent<TextMesh> ().text = collisionObject.name;
			//distance = hitInfo.distance;
			if(collisionObject.tag.Equals("Category")){
				Vector3 desiredDestinationPosition = collisionObject.transform.position;
				desiredDestinationPosition.y += 1; //offset the y so we arrive at the top of the planet
				moveUserToPosition(desiredDestinationPosition, 0.5f);
				Debug.Log("Hit Planet");
			} else if (collisionObject.tag.Equals("Video")){
				Handheld.PlayFullScreenMovie("Uncharted.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
			}

		} else {
			//distance = maxCrosshairDist;
		}

		if (inputDetector.backButtonIsClicked ()) {
			moveUserToPosition(startingPosition, 0);
		}

		if(moveCamera){
			float step = speed * Time.deltaTime;
			userObject.transform.position = Vector3.MoveTowards(userObject.transform.position, destinationPosition, step);

			//user has reached the object
			if((userObject.transform.position-destinationPosition).sqrMagnitude <= destinationOffset){
				moveCamera = false;
			}

		}
	}

	private void moveUserToPosition(Vector3 destinationPosition, float destinationOffset) {
		moveCamera = true;
		this.destinationPosition = destinationPosition;
		this.destinationOffset = destinationOffset;
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
