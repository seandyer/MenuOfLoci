using UnityEngine;
using System.Collections;

public class InputDetector : MonoBehaviour {

	public float tapThreshold = 50; //size before click becomes a swipe

	private bool isTapped;
	private bool isSwiped;
	private float swipeX;
	private float swipeY;
	private Vector3 touchPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		checkTap ();
	}

	//True if the user has clicked the back button
	public bool backButtonIsClicked() {
		return Input.GetMouseButtonUp (1);
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
				swipeX = Input.GetAxis ("Mouse X");
				swipeY = Input.GetAxis ("Mouse Y");
			}
		}
	}

	public float getSwipeXValue() {
		return swipeX;
	}
	
	public float getSwipeYValue() {
		return swipeY;
	}

	public bool touchpadIsSwiped() {
		return isSwiped;
	}

	public bool touchpadIsTapped() {
		return isTapped;
	}
}
