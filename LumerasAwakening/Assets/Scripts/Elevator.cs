using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
    public bool canMove;
    public float weight;
    public Rigidbody rb;
    public float force;
    // Use this for initialization
    void Start () {
        canMove = true;
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.AddForce(0, force, 0, ForceMode.Force);
            //float upwardsVelocity = .1f;
            //Vector3 movement = new Vector3(0.0f, 0.1f, 0.0f);
            //transform.position += movement;

        }
    }
}
