using UnityEngine;
using System.Collections;
using SimpleJSON;

public class WebLoader : MonoBehaviour {

	private bool isReadFinished;
	private int itemCount;
	private string[] thumbnailFileNames;
	private string[] videoFileNames;

	// Use this for initialization
	void Start () {
	
		//loadCategory ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int getNumberOfItemsInCategory() {
		return itemCount;
	}

	public string[] getThumbnailURLS() {
		string[] thumbnailURLS = new string[thumbnailFileNames.Length];
		for (int i = 0; i < thumbnailURLS.Length; i++) {
			thumbnailURLS[i] = "http://ec2-52-26-4-14.us-west-2.compute.amazonaws.com/thumbnails/" + thumbnailFileNames[i];
		}

		return thumbnailURLS;
	}

	public string[] getVideoURLS() {
		string[] videoURLS = new string[videoFileNames.Length];
		for (int i = 0; i < videoURLS.Length; i++) {
			videoURLS[i] = "http://ec2-52-26-4-14.us-west-2.compute.amazonaws.com/video/" + videoFileNames[i];
		}

		return videoURLS;
	}

	public void loadCategory(string categoryName) {
		StartCoroutine (loader (categoryName));
	}

	public IEnumerator loader(string categoryName){
		isReadFinished = false;

		WWW w = new WWW("http://ec2-52-26-4-14.us-west-2.compute.amazonaws.com:8080/restvr/webapi/movies/" + categoryName);
		yield return w;
		
		var N = JSONNode.Parse(w.text);
	
		itemCount = N.Count;
		thumbnailFileNames = new string[N.Count];
		videoFileNames = new string[N.Count];

		for(int count=0; count< N.Count; count++) {

			string videoNumber = N[count]["videonum"];
			thumbnailFileNames[count] =  N[count]["thumbnail"];
			videoFileNames[count] =  N[count]["video"]; 
			string category =  N[count]["category"]; 

			/*
			Debug.Log("Video number : "+videoNumber);
			Debug.Log("Thumbnail : "+thumbnail);
			Debug.Log("Video : "+video);
			Debug.Log("Category : "+category);
			*/

		}
		isReadFinished = true;
	}
}
