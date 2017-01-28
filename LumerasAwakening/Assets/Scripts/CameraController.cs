using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private Vector3 offset;
    private float distance;
    private float angle;
    public float panSpeed;
    // Use this for initialization
    void Start()
    {
        //Determines the distance between the player and the camera
        offset = transform.position - player.transform.position;
        //Angle of the camera with respect to vertices x,z to the player
        angle = (float) Math.Atan(offset.z/offset.x);
        //Distance between the player and the camera with respect to vertices x,z
        distance = (float) Math.Sqrt(offset.z*offset.z +offset.x*offset.x);
        //This is needed to initially face the correct direction
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
    }

    // FixedUpdate is called once per fixed increment
    void FixedUpdate()
    {

        //Get input
        float pan = -Input.GetAxis("CameraPan") * panSpeed;

        //Recalculate the cameras angle to the player
        angle += pan * 0.01f;
        if (angle > Math.PI)
        {
            angle -= 2.0f * (float)Math.PI;
        }
        else if (angle < -Math.PI)
        {
            angle += 2.0f * (float)Math.PI;
        }

        //Update the offset after panning
        offset = new Vector3((float)Math.Cos(angle) * distance, offset.y, (float)Math.Sin(angle) * distance);
        transform.position = player.transform.position + offset;
        //Rotate camera to still face the player object
        transform.Rotate(0.0f, -360.0f * (pan * 0.01f / (2.0f * (float)Math.PI)), 0.0f, Space.World);
    }
}
