using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upFunZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if(transform.GetComponent<Renderer>().materials.Length == 2)
        {
            Destroy(transform.GetComponent<Renderer>().materials[1]);
        }
    }
}
