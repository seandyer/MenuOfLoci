using UnityEngine;
using System.Collections;

public class Thumbnail : MonoBehaviour {

	private bool textureLoaded = false;
	private Renderer rend;
	private string thumbnailFileName;
	private string videoFileName;
	private Texture2D tex;
	// Use this for initialization

	void Awake() {
		rend = this.GetComponent<Renderer> ();
		rend.enabled = true;
	}

	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
	
	}

	public string getVideoFileName() {
		return videoFileName;
	}

	public bool isTextureLoaded() {
		return textureLoaded;
	}

	public void loadTexture() {
		StartCoroutine (setTexture (thumbnailFileName));
	}

	public void setInvisible() {
		rend.enabled = false;
	}

	public void setVisible() {
		rend.enabled = true;
	}

	public void setImageFileNameWithoutLoading(string fileName) {
		thumbnailFileName = fileName;
	}

	public void setImageFileName(string fileName) {
		thumbnailFileName = fileName;
		StartCoroutine (setTexture (fileName));
	}

	private IEnumerator setTexture(string fileName) {
		WWW wwwTexture = new WWW (fileName);
		yield return wwwTexture;

		tex = new Texture2D(250,250);
		tex = (Texture2D)wwwTexture.texture;

		rend.material.mainTexture = tex;
		textureLoaded = true;
	}

	public void setVideoFileName(string fileName) {
		this.videoFileName = fileName;
	}
}
