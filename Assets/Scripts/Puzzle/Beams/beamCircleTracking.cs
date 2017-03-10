using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beamCircleTracking : MonoBehaviour {
    public GameObject targetObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(targetObject.GetComponent<Renderer>().bounds.center.x, targetObject.GetComponent<Renderer>().bounds.max.y, targetObject.GetComponent<Renderer>().bounds.center.z);
    }
}
