using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerCamera;
    private Vector3 offset;
    public float speed;
    public float jumpHeight;
    private Rigidbody rb;
    private float angle;
    private bool canJump;
    public float panSpeed;

    // Use this for initialization
    void Start()
    {
        //Determines the distance between the player and the camera
        offset = playerCamera.transform.position - transform.position;
        //Angle of the camera with respect to vertices x,z to the player
        angle = (float)(Math.PI + Math.Atan(offset.z / offset.x));
        rb = GetComponent<Rigidbody>();
        //Possible to jump
        canJump = true;
        speed *= 0.1f;
    }

    // FixedUpdate is called once per fixed increment
    void FixedUpdate()
    {
        rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
        rb.angularVelocity = new Vector3(0.0f, rb.angularVelocity.y, 0.0f);
        //Get inputs
        float moveHorizontal = -Input.GetAxis("Horizontal");
        float moveVertical = -Input.GetAxis("Vertical");
        bool jumpPressed = Input.GetButtonDown("Jump");
        float pan = -Input.GetAxis("CameraPan") * panSpeed;

        //If it is possible to jump then jump
        if (jumpPressed && canJump)
        {
            Vector3 jump = new Vector3(0.0f, jumpHeight * 10, 0.0f);
            rb.AddForce(jump);
        }

        //Recalculate the cameras angle to the player
        angle += pan * 0.01f;
        if (angle > Math.PI)
        {
            angle -= 2.0f * (float) Math.PI;
        }
        else if (angle < -Math.PI)
        {
            angle += 2.0f * (float) Math.PI;
        }

        float horizontal = (float)Math.Cos(angle) * moveVertical + (float)Math.Sin(angle) * moveHorizontal;
        float vertical = (float)Math.Sin(angle) * moveVertical - (float)Math.Cos(angle) * moveHorizontal;

        //translate the player
        Vector3 movement = new Vector3(horizontal * speed, 0.0f, vertical * speed);
        transform.position += movement;
    }

    void OnTriggerEnter(Collider other)
    {
        //If you are on the floor you can jump
        if (other.gameObject.CompareTag("Floor"))
        {
            canJump = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //If you are not on the floor you can not jump
        if (other.gameObject.CompareTag("Floor"))
        {
            canJump = false;
        }
    }
}
