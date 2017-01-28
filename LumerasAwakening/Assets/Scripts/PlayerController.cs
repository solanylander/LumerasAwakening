using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpHeight;
    private Rigidbody rb;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        speed *= 0.1f;
    }
	
	// Update is called once per fixed increment
	void FixedUpdate () {
        float moveHorizontal = -Input.GetAxis("Horizontal");
        float moveVertical = -Input.GetAxis("Vertical");
        bool jumpPressed = Input.GetButtonDown("Jump");
        Vector3 movement = new Vector3(moveHorizontal * speed, 0.0f, moveVertical * speed);
        transform.position += movement;

        if (jumpPressed)
        {
            Vector3 jump = new Vector3(0.0f, jumpHeight * 10, 0.0f);
            rb.AddForce(jump);
        }
    }
}
