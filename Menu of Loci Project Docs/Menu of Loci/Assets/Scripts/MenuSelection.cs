using UnityEngine;
using System.Collections;

public class MenuSelection : MonoBehaviour {
	
	public Camera cameraObject; //Add the camera object to this slot in the Inspector
	public GameObject scriptObject; //Add the singleton script game object to this slot in the Inspector
	
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
			GameObject.Find ("DebugText").GetComponent<TextMesh> ().text = raycastForObject ().name;
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
