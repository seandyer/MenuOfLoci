using UnityEngine;
using System.Collections;

public class Thumbnail : MonoBehaviour {

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

	public void setInvisible() {
		rend.enabled = false;
	}

	public void setVisible() {
		rend.enabled = true;
	}

	public void setImageFileName(string fileName) {
		thumbnailFileName = fileName;
		StartCoroutine (setTexture (fileName));
	}

	private IEnumerator setTexture(string fileName) {
		WWW wwwTexture = new WWW (fileName);
		yield return wwwTexture;

		tex = new Texture2D(500,500);
		tex = (Texture2D)wwwTexture.texture;
		rend.material.mainTexture = tex;
	}

	public void setVideoFileName(string fileName) {
		this.videoFileName = fileName;
	}
}
