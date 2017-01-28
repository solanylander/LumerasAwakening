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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float pan = Input.GetAxis("CameraPan");
        angle += pan * 0.01f;
        if(angle > Math.PI)
        {
            angle -= (float) Math.PI;
        }else if(angle < 0.0f)
        {
            angle += (float)Math.PI;
        }
        Debug.Log(angle);
        offset = new Vector3((float) Math.Cos(angle) / distance, offset.y, (float)Math.Sin(angle) / distance);
        transform.position = player.transform.position + offset;
    }
}
