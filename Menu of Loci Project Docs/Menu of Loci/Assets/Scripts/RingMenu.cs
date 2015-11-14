using UnityEngine;
using System.Collections;

public class RingMenu : MonoBehaviour {

	public float displacementAngle = 45;
	public GameObject thumbnailObject; //Attach the thumbnail object to this slot in the Inspector
	public GameObject userObject; //Attach the user/navigation object to this slot in the Inspector

	private GameObject[] thumbnailArray;
	
	// Use this for initialization
	void Start () {
		spawnThumbnails();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown("space")){

			foreach (GameObject thumbnailObject in thumbnailArray) {
				thumbnailObject.transform.RotateAround(userObject.transform.position, Vector3.up, 200 * Time.deltaTime);
			}
			//Vector3 oldVector = userObject.transform.eulerAngles;
			//userObject.transform.eulerAngles = new Vector3(oldVector.x,oldVector.y+45,oldVector.z);
			
		}
	}
	
	public void spawnThumbnails() {
		//thumbnailObject = Instantiate(Resources.Load("Thumbnail")) as GameObject;
		int thumbnailNum = (int) (360 / displacementAngle);
		thumbnailArray = new GameObject[thumbnailNum];

		for (int i = 0; i < thumbnailNum; i++) {
			Vector3 position = userObject.transform.position;
			position.z += 2;
			//Quaternion rotation = Quaternion.identity;
			//position.
			//rotation.SetLookRotation(userObject.transform.position);
			GameObject spawnedObject = Instantiate(thumbnailObject, position, Quaternion.identity) as GameObject;
			spawnedObject.transform.RotateAround(userObject.transform.position, Vector3.up, displacementAngle * i);
			spawnedObject.transform.rotation.SetLookRotation(userObject.transform.position);
			//Instantiate(thumbnailObject, position, rotation) as GameObject;

			thumbnailArray[i] = spawnedObject;
		}
	}
}
