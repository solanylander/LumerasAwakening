﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beamCircleTracking : MonoBehaviour {
    public GameObject targetObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = targetObject.GetComponent<PrismTest>().rayOrigin;
    }
}
