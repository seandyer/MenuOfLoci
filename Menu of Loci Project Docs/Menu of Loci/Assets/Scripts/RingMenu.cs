using UnityEngine;
using System.Collections;

public class RingMenu : MonoBehaviour {

	public float displacementAngle = 45;
	public GameObject thumbnailObject; //Attach the thumbnail object to this slot in the Inspector
	public GameObject userObject; //Attach the user/navigation object to this slot in the Inspector

	private bool thumbnailsCreated = false;
	private GameObject[] thumbnailArray;
	private string[] videoNames;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown("space") && thumbnailsCreated){

			foreach (GameObject thumbnailObject in thumbnailArray) {
				thumbnailObject.transform.RotateAround(userObject.transform.position, Vector3.up, 200 * Time.deltaTime);
			}
			
		}
	}
	
	public void spawnThumbnails() {
		if (thumbnailsCreated) {
			return;
		}
		//thumbnailObject = Instantiate(Resources.Load("Thumbnail")) as GameObject;
		int thumbnailNum = (int) (360 / displacementAngle);
		thumbnailArray = new GameObject[thumbnailNum];
		Texture2D thumbnailTexture = new Texture2D (500, 500);

		for (int i = 0; i < thumbnailNum; i++) {
			thumbnailTexture = (Texture2D) Resources.Load("Thumbnails/" + (i+1));
			Vector3 position = userObject.transform.position;
			position.z += 2;
			GameObject spawnedObject = Instantiate(thumbnailObject, position, Quaternion.identity) as GameObject;
			spawnedObject.transform.RotateAround(userObject.transform.position, Vector3.up, displacementAngle * i);
			spawnedObject.transform.rotation.SetLookRotation(userObject.transform.position);
			spawnedObject.GetComponent<Renderer>().material.mainTexture = thumbnailTexture;
			thumbnailArray[i] = spawnedObject;
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
