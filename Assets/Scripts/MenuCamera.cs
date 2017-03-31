using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour {
    public float rotateSpeed = 5;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion newRot = Quaternion.Euler(0, transform.eulerAngles.y + 0.025f, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, 1f);
	}
}
