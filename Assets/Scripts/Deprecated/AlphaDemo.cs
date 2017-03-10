using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaDemo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GetComponent<Renderer>().enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}
