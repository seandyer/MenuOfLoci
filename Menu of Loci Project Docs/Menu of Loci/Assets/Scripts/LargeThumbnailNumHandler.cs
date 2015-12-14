using UnityEngine;
using System.Collections;

public class LargeThumbnailNumHandler : MonoBehaviour {

	public int maxNumOfLoadingThumbnails = 30;

	private bool isHandlingLoad = false;
	private GameObject[] loadingThumbnails;
	private GameObject[] thumbnailArray;
	private int nextThumbnailIndex;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isHandlingLoad) {
			replaceFinishedThumbnails();
		}
	}

	private void replaceFinishedThumbnails() {
		bool finishedLoading = true;
		for (int i = 0; i < loadingThumbnails.Length; i++) {
			if (loadingThumbnails[i] != null) {
				finishedLoading = false;
				Thumbnail thumbnailScript = loadingThumbnails[i].GetComponent<Thumbnail> ();
				if (thumbnailScript.isTextureLoaded()) { //if loading is finished, replace
					if (nextThumbnailIndex < thumbnailArray.Length) {
						loadingThumbnails[i] = thumbnailArray[nextThumbnailIndex];
						nextThumbnailIndex++;
						Thumbnail newThumbnailScript = loadingThumbnails[i].GetComponent<Thumbnail> ();
						newThumbnailScript.loadTexture();
					}
					else { //no more thumbnails left to load
						loadingThumbnails[i] = null;
					}
				}
			}
		}
		if (finishedLoading) {
			isHandlingLoad = false;
		}
	}

	public void setThumbnailObjectArray(GameObject[] thumbnailArray) {
		this.thumbnailArray = thumbnailArray;
	}

	public void startLoading() {
		int size = Mathf.Max (maxNumOfLoadingThumbnails, thumbnailArray.Length);
		loadingThumbnails = new GameObject[size];
		for (int i = 0; i < loadingThumbnails.Length; i++) {
			loadingThumbnails [i] = thumbnailArray [i];
			Thumbnail thumbnailScript = loadingThumbnails[i].GetComponent<Thumbnail>();
			thumbnailScript.loadTexture();
		}
		nextThumbnailIndex = loadingThumbnails.Length;
		isHandlingLoad = true;
	}
}
