using UnityEngine;
using System.Collections;

public class InputDetector : MonoBehaviour {

	public float tapThreshold = 50; //size before click becomes a swipe

	private bool isTapped;
	private Vector3 touchPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (checkTap ()) {
			isTapped = true;
		} else {
			isTapped = false;
		}
	}

	private bool checkTap() {
		
		if (Input.GetMouseButtonDown (0)) {
			touchPos = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp (0)) {
			Vector3 delta = Input.mousePosition - touchPos;
			if (delta.sqrMagnitude < tapThreshold * tapThreshold) {
				return true;
			}
		}
		return false;
	}

	public bool touchpadIsTapped() {
		return isTapped;
	}
}
