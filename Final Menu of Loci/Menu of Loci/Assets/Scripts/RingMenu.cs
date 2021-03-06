﻿using UnityEngine;
using System.Collections;

public class RingMenu : MonoBehaviour {

	public float displacementAngle = 45;
	public float startingDisplacement = -45;
	public float distanceFromUser = 2;
	public float levelHeight = 1;
	public int maxThumbnailsPerLevel = 8;
	public float ySwipeScale = 0.1f;
	public float menuMovementAcceleration = 0.1f;
	public float menuMovementMaxVelocity = 1;
	public GameObject singletonScripts; //Attach the singleton script gameOBject to this slot in the Inspector
	public GameObject thumbnailObject; //Attach the thumbnail object to this slot in the Inspector
	public GameObject userObject; //Attach the user/navigation object to this slot in the Inspector

	private bool menuMoving = false;
	private bool thumbnailsCreated = false;
	private bool thumbnailsGenerating = false;
	private GameObject[] thumbnailArray;
	private float menuYOffset = 0; //the y distance the menu has moved down
	private float curMenuVelocity = 0;
	private int curThumbnailNum;
	private int curNumOfLevels;
	private int desiredMenuLevel = 0;
	private InputDetector inputDetector;
	private string[] videoNames;
	private WebLoader webLoader;

	// Use this for initialization
	void Start () {
		if (singletonScripts != null) {
			inputDetector = (InputDetector) singletonScripts.GetComponent("InputDetector");
			webLoader = singletonScripts.GetComponent<WebLoader>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown("space") && thumbnailsCreated){

			foreach (GameObject thumbnailObject in thumbnailArray) {
				thumbnailObject.transform.RotateAround(userObject.transform.position, Vector3.up, 200 * Time.deltaTime);
			}
			
		}
		if (thumbnailsCreated) {
			if (inputDetector.touchpadIsSwiped ()) {
//				GameObject.Find ("DebugText").GetComponent<TextMesh> ().text = inputDetector.getSwipeYValue ().ToString ();
				moveMenuByAmount(inputDetector.getSwipeYValue() * ySwipeScale * -1);
				/*
				if (inputDetector.getSwipeYValue () > 0) {
					moveDesiredMenuLevelUp ();
				} else {
					moveDesiredMenuLevelDown ();
				}
				*/
			}
			//processMenuMovement ();
		}
	}
	
	public IEnumerator spawnThumbnails(string category, Vector3 position) {
		if (thumbnailsCreated) {
			yield break;
		}
		thumbnailsGenerating = true;
		//thumbnailObject = Instantiate(Resources.Load("Thumbnail")) as GameObject;
		//thumbnailNum = (int) (360 / displacementAngle);
		yield return StartCoroutine(webLoader.loader(category));
		//StartCoroutine(webLoader
		int thumbnailNum = webLoader.getNumberOfItemsInCategory();
		string[] thumbnailFileNames = webLoader.getThumbnailURLS ();
		string[] videoFileNames = webLoader.getVideoURLS ();

		thumbnailArray = new GameObject[thumbnailNum];
		Texture2D thumbnailTexture = new Texture2D (500, 500);

		curThumbnailNum = thumbnailNum;
		curNumOfLevels = thumbnailNum / maxThumbnailsPerLevel + 1;
		for (int i = 0; i < curNumOfLevels; i++) { 
			int thumbnailNumInLevel;
			if (i + 1 == curNumOfLevels) { //we are at the last level
				thumbnailNumInLevel = thumbnailNum % maxThumbnailsPerLevel;
			}
			else {
				thumbnailNumInLevel = maxThumbnailsPerLevel;
			}
			//WebLoader webLoader = new WebLoader();
			for (int j = 0; j < thumbnailNumInLevel; j++) {
				int thumbnailArrayNumber = i * maxThumbnailsPerLevel + j;
				//thumbnailTexture = (Texture2D) Resources.Load("Thumbnails/" + (thumbnailArrayNumber+1));
				Vector3 userPosition = position;
				Vector3 spawnPosition = position + Vector3.up * i * levelHeight; //at the user position and up the number of levels
				spawnPosition.z += distanceFromUser;
				GameObject spawnedObject = Instantiate(thumbnailObject, spawnPosition, Quaternion.identity) as GameObject;
				spawnedObject.transform.RotateAround(userPosition, Vector3.up, startingDisplacement + displacementAngle * j);
				spawnedObject.transform.rotation.SetLookRotation(userPosition);
				Thumbnail thumbnailScript = spawnedObject.GetComponent<Thumbnail>();
				thumbnailScript.setInvisible();
				if (thumbnailNum < 40) {
					thumbnailScript.setImageFileName(thumbnailFileNames[thumbnailArrayNumber]);
				}
				else { //thumbnail array is oversized
					thumbnailScript.setImageFileNameWithoutLoading(thumbnailFileNames[thumbnailArrayNumber]);
				}
				thumbnailScript.setVideoFileName(videoFileNames[thumbnailArrayNumber]);
				//spawnedObject.GetComponent<MeshRenderer>().enabled = false;
				thumbnailArray[thumbnailArrayNumber] = spawnedObject;
				//spawnedObject.SetActive(false);
			}
		}
		if (thumbnailNum >= 40) { //thumbnail array is oversized
			LargeThumbnailNumHandler handler = singletonScripts.GetComponent<LargeThumbnailNumHandler> ();
			handler.setThumbnailObjectArray (thumbnailArray);
			handler.startLoading ();
		}
		thumbnailsCreated = true;
		thumbnailsGenerating = false;
	}

	public IEnumerator makeThumbnailsVisible() {
		while (thumbnailsGenerating) {
			yield return new WaitForSeconds (0.1f);
		}
		foreach (GameObject thumbnail in thumbnailArray) {
			Thumbnail thumbnailScript = thumbnail.GetComponent<Thumbnail>();
			thumbnailScript.setVisible();
		}
	}

	public void despawnThumbnails() {
		if (!thumbnailsCreated) {
			return;
		}

		for (int i = 0; i < thumbnailArray.Length; i++) {
			Destroy(thumbnailArray[i]);
		}
		thumbnailsCreated = false;
	}

	private void moveMenuByAmount(float amount) {
		float movement;
		if (amount > 0) { //we are moving up
			float maxMenuYOffset = (curNumOfLevels - 1) * levelHeight;
			if (menuYOffset == maxMenuYOffset) {
				return;
			}
			if (menuYOffset + amount < maxMenuYOffset) {
				movement = amount;
			} else { //overshoot
				movement = maxMenuYOffset - menuYOffset;
			}
		} else { //we are moving down
			if (menuYOffset == 0) {
				return;
			}
			if (menuYOffset + amount > 0) {
				movement = amount;
			}
			else {
				movement = menuYOffset * -1;
			}
		}
		foreach (GameObject thumbnail in thumbnailArray) {
			thumbnail.transform.Translate(Vector3.up * movement * -1);
		}
		menuYOffset += movement;
		//Debug.Log (menuYOffset);
	}

	private void moveDesiredMenuLevelDown() {
		if (desiredMenuLevel < curNumOfLevels) {
			desiredMenuLevel++;
			menuMoving = true;
		}
	}

	private void moveDesiredMenuLevelUp() {
		if (desiredMenuLevel > 0) {
			desiredMenuLevel--;
			menuMoving = true;
		}
	}

	private void processMenuMovement() {
		if (menuMoving) {
			float desiredYOffset = desiredMenuLevel * levelHeight;
			float newMenuYOffset = menuYOffset;
			if (menuYOffset != desiredYOffset) {
				if (desiredYOffset > menuYOffset) { //menu still needs to move down
					newMenuYOffset += curMenuVelocity; //increase offset
					if (newMenuYOffset > desiredYOffset) { //if overshot
						newMenuYOffset = desiredYOffset;
					}
				}
				else { //menu needs to move up
					newMenuYOffset -= curMenuVelocity;
					if (newMenuYOffset < desiredYOffset) {
						newMenuYOffset = desiredYOffset;
					}
				}
				float movement = (newMenuYOffset - menuYOffset) * -1; //get the distance and negate it to move in the correct direction
				//move all the menu thumbnails
				foreach (GameObject thumbnail in thumbnailArray) {
					thumbnail.transform.Translate(Vector3.up * movement);
				}
	         	menuYOffset = newMenuYOffset;
				//apply acceleration
				curMenuVelocity += menuMovementAcceleration * Time.deltaTime;
				if (curMenuVelocity > menuMovementMaxVelocity) {
					curMenuVelocity = menuMovementMaxVelocity;
				}
			}
			else {
				menuMoving = false;
				curMenuVelocity = 0;
			}
		}
	}
}