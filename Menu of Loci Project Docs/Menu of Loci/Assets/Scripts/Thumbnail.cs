using UnityEngine;
using System.Collections;

public class Thumbnail : MonoBehaviour {

	//private Renderer meshRenderer;
	private string thumbnailFileName;
	private string fileName;
	private Texture2D tex;
	// Use this for initialization
	void Start () {
		//meshRenderer = this.gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	public void setInvisible() {
		meshRenderer.enabled = false;
	}

	public void setVisible() {
		meshRenderer.enabled = true;
	}
		*/

	public void setImageFileName(string fileName) {
		thumbnailFileName = fileName;
		StartCoroutine (setTexture (fileName));
	}

	private IEnumerator setTexture(string fileName) {
		WWW wwwTexture = new WWW ("http://ec2-52-26-4-14.us-west-2.compute.amazonaws.com/thumbnails/" + fileName);
		yield return wwwTexture;

		tex = new Texture2D(500,500);
		tex = (Texture2D)wwwTexture.texture;


		this.GetComponent<Renderer>().material.mainTexture = tex;
	}
}
