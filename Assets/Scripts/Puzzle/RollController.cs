using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollController : MonoBehaviour {

    Vector3 Position;

	// Use this for initialization
	void Start () {
        Position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Teleporter")
        {
            transform.position = Position;
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
