﻿using UnityEngine;
using System.Collections;

public class Planet1Rotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		this.transform.Rotate(new Vector3(0,0.1f,0));
	}
}
