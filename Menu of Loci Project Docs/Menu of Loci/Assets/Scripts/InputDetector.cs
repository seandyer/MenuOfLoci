using UnityEngine;
using System.Collections;

public class InputDetector : MonoBehaviour {

	public float tapThreshold = 50; //size before click becomes a swipe

	private bool isTapped;
	private bool isSwiped;
	private Vector3 touchPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		checkTap ();
	}
	// Update isTapped and isSwiped if the touchpad is touched and released
	private void checkTap() {

		isTapped = false;
		isSwiped = false;
		if (Input.GetMouseButtonDown (0)) {
			touchPos = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp (0)) {
			Vector3 delta = Input.mousePosition - touchPos;
			if (delta.sqrMagnitude < tapThreshold * tapThreshold) { //release is within the threshold
				isTapped = true;
			}
			else {
				isSwiped = true;
			}
		}
	}

	public bool touchpadIsSwiped() {
		return isSwiped;
	}

	public bool touchpadIsTapped() {
		return isTapped;
	}
}
