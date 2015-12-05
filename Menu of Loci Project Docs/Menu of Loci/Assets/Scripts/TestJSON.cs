using UnityEngine;
using System.Collections;
using SimpleJSON;

public class TestJSON : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		StartCoroutine(loadJson());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator loadJson(){

		WWW w = new WWW("http://ec2-52-26-4-14.us-west-2.compute.amazonaws.com:8080/restvr/webapi/Test");
		yield return w;

		var N = JSONNode.Parse(w.text);

		string videoNumber = N[0]["videonum"];
		string thumbnail =  N[0]["thumbnail"];
		string video =  N[0]["video"]; 
		string category =  N[0]["category"]; 

		Debug.Log("Video number : "+videoNumber);
		Debug.Log("Thumbnail : "+thumbnail);
		Debug.Log("Video : "+video);
		Debug.Log("Category : "+category);
	}
}
