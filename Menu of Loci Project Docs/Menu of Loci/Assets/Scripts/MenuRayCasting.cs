using UnityEngine;
using System.Collections;

// Notes:
// Attach to any GameObject

public class MenuRayCasting : MonoBehaviour {

	// Public variables that are customizable in the Inspector
	public float rayRadius = 0.025f;
	public float rayMaxLength = 100.0f;
	public float maxDistance = 20.0f; //Max distance from a category planet. Will have to tweak based on our scale.
	// State variables and data required among the functions of the class
	bool moving = false;
	bool manipulating = false;
	bool inMenu = false;
	float objectDistance = 0.0f;
	GameObject raycastObject = null;
	GameObject menu = null;
	GameObject originalObject= null;
	Transform selectedPlanet = null;
	Transform grabbedObject = null;
	Transform menuObject = null;
	Renderer menuRenderer;
	// Use this for initialization
	void Start () {
		//menuRenderer = GameObject.Find ("MenuTest").GetComponent<Renderer> ();
		menu = GameObject.Find ("Menu");
		menuObject = menu.transform;
		// Get reference to empty GameObject located under the Main Camera.
		// This object serves as the moving point relative to the grabbed object.
		grabbedObject = GameObject.Find("Grabbed Object").transform;
	
		// Color the ray GameObject blue by default
		GameObject.Find("Ray").GetComponent<Renderer>().material.color = Color.blue;
	
	}
	/*So our states are: 
	 * not moving, not in menu: normal raycasting and object selection
	 * in menu: raycasting to select a menu object. We need to remember the selected object
	 * moving: 
	 **/
	// Update is called once per frame
	void Update () {
	
		// If an object is not being grabbed
		if(!moving) {
			// Update the ray based on the gaze direction
			UpdateRay();
			// Then check to see if the user is selecting anything
			// (either an object with a "Category" tag or
			// a "File" tag.
			CheckSelect(); //Original method was check grb.
		}
		// If an object is being grabbed
		else {
			// Move the user towards the chosen category
			MoveUser(); //Original method was manipulateObject()
			// Check to see if it is time to stop the user from moving
			CheckStop(); //Original method was checkrelease
		}
	
	}
	
	// Update the ray based on the gaze direction
	void UpdateRay () {

		if (!inMenu) {
			Vector3 currentMenuPos = menuObject.transform.position;
			menuObject.transform.position = new Vector3(currentMenuPos.x, currentMenuPos.y, currentMenuPos.z-10);
		}
		//GameObject.Find("Ray").GetComponent<Renderer>().material.color = Color.black;
		// Get the head position and gaze direction from the Main Camera object
		Vector3 headPosition = GameObject.Find("Main Camera").transform.position;
		Vector3 gazeDirection = GameObject.Find("Main Camera").transform.forward;
		
		// Use the hand position and ray objects to make technique appear like traditional raycasting
		Transform handPosition = GameObject.Find("Hand Position").transform;
		GameObject rayObject = GameObject.Find("Ray");
		
		// Prepare to capture intersection data
		RaycastHit hitInfo;
		
		// Have the physics engine check for intersections with the vector originating at the head position
		// and heading in the direction of the user's gaze
		if(Physics.Raycast(headPosition, gazeDirection, out hitInfo)) {
		
			// Get the object being intersected

			raycastObject = hitInfo.transform.gameObject;

			//We need to have a separate menu object selected here,
			//so raycastObject if not in menu mode, menuObject if in menu mode
			// Move the relative grab point to the intersection point
			grabbedObject.transform.position = hitInfo.point;
			
			// Keep track of the original distance to the object
			objectDistance = (grabbedObject.transform.position - headPosition).magnitude;
			
			// Now update the ray
			
			// As the hand position object is holding the ray, we'll point it to the intersected object
			handPosition.LookAt(hitInfo.point);
			
			// Calculate the vector that will represent the ray and that vector's length
			Vector3 rayVector = hitInfo.point - handPosition.position;
			float rayLength = rayVector.magnitude;
			
			// Scale and position the ray object accordingly to the hand position
			rayObject.transform.localPosition = new Vector3(0.0f, 0.0f, rayLength/2.0f);
			rayObject.transform.localScale = new Vector3(rayRadius, rayLength/2.0f, rayRadius);
			
			// Provide feedback whether the object can be manipulated
			// Only objects with the Manipulable tag can be manipulated
			if(raycastObject.tag == "Manipulable" && !inMenu) {
				// Indicate manipulable objects with a green ray
			
				rayObject.GetComponent<Renderer>().material.color = Color.green;
//				if (inMenu) {
//					rayObject.GetComponent<Renderer>().material.color = Color.black;
//				}

			} else if (inMenu) {
				if (raycastObject.tag == "Grab" ||raycastObject.tag == "Color" ||raycastObject.tag == "Bigger" ||raycastObject.tag == "Smaller") {
					rayObject.GetComponent<Renderer>().material.color = Color.white;
				} else {
					rayObject.GetComponent<Renderer>().material.color = Color.black;
				}

			}
			// Not manipulable 
			else {
				// Indicate with a default blue ray
				rayObject.GetComponent<Renderer>().material.color = Color.blue;
//				if(menuObject == null) {
//					rayObject.GetComponent<Renderer>().material.color = Color.black;
//				}
			}
		}
		// No object was intersected by the user's gaze
		else {
		
			// Indicate a null object to ensure we don't try to fetch properties of it
			raycastObject = null;
			
			// Have the ray point in whatever direction the Main Camera is pointing
			handPosition.localRotation = GameObject.Find("Main Camera").transform.rotation;
			
			// Have the ray extend out to the maximum length
			rayObject.transform.localPosition = new Vector3(0.0f, 0.0f, rayMaxLength/2.0f);
			rayObject.transform.localScale = new Vector3(rayRadius, rayMaxLength/2.0f, rayRadius);
			
			// Show the default blue ray
			rayObject.GetComponent<Renderer>().material.color = Color.blue;
		}
	}
	
	// Check to see if the user is moving the object with a touchpad press
	void CheckGrab () {
		
		// If the touchpad is being pressed and an object is being intersected and it is manipulable
		if(Input.GetMouseButton(0) && raycastObject != null && raycastObject.tag == "Manipulable" && !inMenu) {
			inMenu = true;
			GameObject.Find("Ray").GetComponent<Renderer>().material.color = Color.black;
			Transform rayTransform = raycastObject.transform;
			menuObject.position = new Vector3(rayTransform.position.x+0.5f, rayTransform.position.y, rayTransform.position.z*0.5f);
			if(menuObject.position.z < 2.5f) {
				menuObject.position = new Vector3(menuObject.position.x+0.5f, menuObject.position.y, 2.5f);
			}
			originalObject = raycastObject;
//
//			// Change our state to moving
//			
		}  else if(Input.GetMouseButton(0) && raycastObject != null && inMenu) {
			if (raycastObject.tag == "Grab") {
				moving = true;
						
				// Move the object under the empty grabbed object within the hierarchy to keep relative placement
				originalObject.transform.parent = grabbedObject;
				
				// Turn off the object's kinematics to avoid the object from spinning in place due to collisions
				originalObject.GetComponent<Rigidbody>().isKinematic = true;
				
				// Color the ray yellow to indicate that an object is grabbed
				GameObject.Find("Ray").GetComponent<Renderer>().material.color = Color.yellow;
				inMenu = false;
				Vector3 currentMenuPos = menuObject.transform.position;
				menuObject.transform.position = new Vector3(currentMenuPos.x, currentMenuPos.y, currentMenuPos.z-10);
				originalObject.transform.position = currentMenuPos;
			} else if (raycastObject.tag == "Color") {
				Color choiceColor = raycastObject.GetComponent<Renderer>().material.color;
				originalObject.GetComponent<Renderer>().material.color = choiceColor;

			} else if (raycastObject.tag == "Bigger") {
				Transform currentDimensions = originalObject.transform;
				originalObject.transform.localScale = currentDimensions.localScale * 1.1f;
			} else if (raycastObject.tag == "Smaller") {
				Transform currentDimensions = originalObject.transform;
				originalObject.transform.localScale = currentDimensions.localScale * 0.9f;
			}
		} else if(Input.GetMouseButton(1) && inMenu) {
			raycastObject = null;
			originalObject = null;
			inMenu = false;
		}
	}
	
	
	// Manipulate the object and ray based on the user's input
	void ManipulateObject () {
	
		// Get any forward or backward swipes from the touchpad
		float SwipeX = Input.GetAxis("Mouse X");
		
		// If there are any swipes and the touchpad was already pressed
		if(Mathf.Abs(SwipeX) != 0.0f && manipulating) {
			
			// Move the object forward or backward based on the swipe
			objectDistance -= SwipeX;
			
			// Keep the object a minimum distance of 1 meter away
			if(objectDistance < 1.0f) {
				objectDistance = 1.0f;
			}
		}
		
		// IMPORTANT: Swipes are based on the last touched position and the newest touched position.
		// Hence, if the user stopes touching the touchpad and then touches it again, a swipe from the
		// last touched point to the new touched point is generated. This will counteract any previous
		// input based on swipes, if you're not careful to check for this situation.
		
		// Check if the touchpad is being pressed
		if(Input.GetMouseButton(0)) {
			// Then we can accept a swipe next frame
			manipulating = true;
		}
		// If the touchpad is not being pressed
		else {
			// We will not accept a swipe next frame
			manipulating = false;
		}
		
		// Update the object's location based on the new object distance
		Vector3 objectLocation = grabbedObject.localPosition;
		objectLocation.z = objectDistance;
		grabbedObject.localPosition = objectLocation;
		
		// Use the hand position and ray objects to make technique appear like traditional raycasting
		Transform handPosition = GameObject.Find("Hand Position").transform;
		GameObject rayObject = GameObject.Find("Ray");
		
		// Point the hand position to the grabbed object's location
		handPosition.LookAt(grabbedObject.position);
		
		// Calculate the vector that will represent the ray and that vector's length
		Vector3 rayVector = grabbedObject.position - handPosition.position;
		float rayLength = rayVector.magnitude;
		
		// Scale and position the ray object accordingly to the hand position
		rayObject.transform.localPosition = new Vector3(0.0f, 0.0f, rayLength/2.0f);
		rayObject.transform.localScale = new Vector3(rayRadius, rayLength/2.0f, rayRadius);
	}
	
	// Check if the user is releasing the object with a back button press
	void CheckStop () {
		Vector3 headPosition = GameObject.Find("Main Camera").transform.position;
		float planetDistance = Vector3.Distance (headPosition, selectedPlanet.position);

		if(planetDistance <= maxDistance) { 
			
			// Change our state back to not moving
			moving = false;
			selectedPlanet.position = null;

			//Here goes code to display the video previews/thumbnails in that category

		}
	}
}
