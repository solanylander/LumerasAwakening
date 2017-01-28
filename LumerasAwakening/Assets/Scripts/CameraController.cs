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
    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
        angle = (float) Math.Atan(offset.z/offset.x);
        distance = (float) Math.Sqrt(offset.z*offset.z +offset.x*offset.x);
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float pan = Input.GetAxis("CameraPan");
        angle += pan * 0.01f;
        if (angle > Math.PI)
        {
            angle -= 2.0f * (float)Math.PI;
        }
        else if (angle < -Math.PI)
        {
            angle += 2.0f * (float)Math.PI;
        }
        offset = new Vector3((float)Math.Cos(angle) * distance, offset.y, (float)Math.Sin(angle) * distance);
        transform.position = player.transform.position + offset;
        transform.Rotate(0.0f, -360.0f * (pan * 0.01f / (2.0f * (float)Math.PI)), 0.0f, Space.World);
    }
}
