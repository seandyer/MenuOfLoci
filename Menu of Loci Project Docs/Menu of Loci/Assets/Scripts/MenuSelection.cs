using UnityEngine;
using System.Collections;

public class MenuSelection : MonoBehaviour {

	public GameObject userObject; //Add the user/navigation object to this slot in the Inspector
	public Camera cameraObject; //Add the camera object to this slot in the Inspector
	public GameObject scriptObject; //Add the singleton script game object to this slot in the Inspector
	public float speed = 10f;

	private GameObject collisionObject;
	private bool moveCamera = false;
	private bool userIsOnPlanet = false;
	private float destinationOffset;
	private Vector3 destinationPosition;
	private Vector3 startingPosition;

	private InputDetector inputDetector;
	private RingMenu ringMenu;
	
	// Use this for initialization
	void Start () {
		if (scriptObject != null) {
			inputDetector = (InputDetector)scriptObject.GetComponent ("InputDetector");
			ringMenu = (RingMenu)scriptObject.GetComponent ("RingMenu");
		}
		if (userObject != null) {
			startingPosition = userObject.transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//User has tapped the touchpad
		if (inputDetector.touchpadIsTapped()) {
			collisionObject = raycastForObject ();
			//GameObject.Find ("DebugText").GetComponent<TextMesh> ().text = collisionObject.name;
			//distance = hitInfo.distance;
			if(collisionObject != null && collisionObject.tag.Equals("Category")){
				Vector3 desiredDestinationPosition = collisionObject.transform.position;
				desiredDestinationPosition.y += 1; //offset the y so we arrive at the top of the planet
				moveUserToPosition(desiredDestinationPosition, 0.5f);
				userIsOnPlanet = true;
				StartCoroutine(ringMenu.spawnThumbnails("Marriott", desiredDestinationPosition)); ///temporary code
				Debug.Log("Hit Planet");
			} else if (collisionObject.tag.Equals("Video")){
				Handheld.PlayFullScreenMovie(collisionObject.GetComponent<Thumbnail>().getVideoFileName(), Color.black, FullScreenMovieControlMode.CancelOnInput);
			}

		} else {
			//distance = maxCrosshairDist;
		}

		if (inputDetector.backButtonIsClicked ()) {
			moveUserToPosition(startingPosition, 0);
			if (userIsOnPlanet) {
				ringMenu.despawnThumbnails();
			}
			userIsOnPlanet = false;
		}

		if(moveCamera){
			//user has reached the object
			if((userObject.transform.position-destinationPosition).sqrMagnitude <= destinationOffset){
				moveCamera = false;
				if (userIsOnPlanet) {
					ringMenu.makeThumbnailsVisible();
				}
			}
			else {
				float step = speed * Time.deltaTime;
				userObject.transform.position = Vector3.MoveTowards(userObject.transform.position, destinationPosition, step);
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
