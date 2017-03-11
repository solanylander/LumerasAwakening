using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour {

    public float speed, time, updown;
    int counter;

	// Use this for initialization
	void Start () {
        counter = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = new Vector3(transform.position.x, transform.position.y + speed * updown, transform.position.z);
        counter++;
        if (counter > time)
        {
            counter = 0;
            updown = updown * -1.0f;
        }

    }
}
