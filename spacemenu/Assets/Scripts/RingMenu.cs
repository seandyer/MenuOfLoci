using UnityEngine;
using System.Collections;

public class RingMenu : MonoBehaviour {

	public float displacementAngle = 45;
	public int maxThumbnailsPerLevel = 8;
	public GameObject singletonScripts; //Attach the singleton script gameOBject to this slot in the Inspector
	public GameObject thumbnailObject; //Attach the thumbnail object to this slot in the Inspector
	public GameObject userObject; //Attach the user/navigation object to this slot in the Inspector

	private bool thumbnailsCreated = false;
	private GameObject[] thumbnailArray;
	private InputDetector inputDetector;
	private string[] videoNames;
	
	// Use this for initialization
	void Start () {
		if (singletonScripts != null) {
			inputDetector = (InputDetector) singletonScripts.GetComponent("InputDetector");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown("space") && thumbnailsCreated){

			foreach (GameObject thumbnailObject in thumbnailArray) {
				thumbnailObject.transform.RotateAround(userObject.transform.position, Vector3.up, 200 * Time.deltaTime);
			}
			
		}
		if (inputDetector.touchpadIsSwiped ()) {
			GameObject.Find ("DebugText").GetComponent<TextMesh> ().text = inputDetector.getSwipeXValue().ToString();
		}
	}
	
	public void spawnThumbnails(int thumbnailNum) {
		if (thumbnailsCreated) {
			return;
		}
		//thumbnailObject = Instantiate(Resources.Load("Thumbnail")) as GameObject;
		//thumbnailNum = (int) (360 / displacementAngle);
		thumbnailArray = new GameObject[thumbnailNum];
		Texture2D thumbnailTexture = new Texture2D (500, 500);

		int numberOfLevels = thumbnailNum / maxThumbnailsPerLevel + 1;
		for (int i = 0; i < numberOfLevels; i++) { 
			int thumbnailNumInLevel;
			if (i + 1 == numberOfLevels) { //we are at the last level
				thumbnailNumInLevel = thumbnailNum % maxThumbnailsPerLevel;
			}
			else {
				thumbnailNumInLevel = maxThumbnailsPerLevel;
			}
			for (int j = 0; j < thumbnailNumInLevel; j++) {
				int thumbnailArrayNumber = i * maxThumbnailsPerLevel + j;
				thumbnailTexture = (Texture2D) Resources.Load("Thumbnails/" + (thumbnailArrayNumber+1));
				Vector3 position = userObject.transform.position + Vector3.up * i;
				position.z += 2;
				GameObject spawnedObject = Instantiate(thumbnailObject, position, Quaternion.identity) as GameObject;
				spawnedObject.transform.RotateAround(userObject.transform.position, Vector3.up, displacementAngle * j);
				spawnedObject.transform.rotation.SetLookRotation(userObject.transform.position);
				spawnedObject.GetComponent<Renderer>().material.mainTexture = thumbnailTexture;
				thumbnailArray[thumbnailArrayNumber] = spawnedObject;
			}
		}
		thumbnailsCreated = true;
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
}
