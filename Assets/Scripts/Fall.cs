using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour {

    Vector3 start;
	// Use this for initialization
	void Start () {
        start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < start.y - 100.0f)
        {
            transform.position = start;
        }
	}
}
