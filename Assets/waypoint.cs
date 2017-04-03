using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypoint : MonoBehaviour
{

    // put the points from unity interface
    public Transform[] wayPointList;

    public int currentWayPoint = 0;
    public float height, sin, initHeight;
    Transform targetWayPoint;

    public float speed;

    // Use this for initialization
    void Start()
    {
        initHeight = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(sin > 360)
        {
            sin = sin % 360.0f;
        }
        sin += speed;
        //Debug.Log(initHeight + Mathf.Sin(sin));
        transform.localPosition = new Vector3(transform.localPosition.x, initHeight + Mathf.Sin(Mathf.PI * (sin / 180.0f)), transform.localPosition.z);
        // check if we have somewere to walk
        if (currentWayPoint < this.wayPointList.Length)
        {
            if (targetWayPoint == null)
                targetWayPoint = wayPointList[currentWayPoint];
            walk();
        }
    }

    void walk()
    {
        // rotate towards the target
       // transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x-targetWayPoint.position.x) < 1.0f && Mathf.Abs(transform.position.z - targetWayPoint.position.z) < 1.0f)
        {
            currentWayPoint++;

            if (currentWayPoint >= this.wayPointList.Length)
            {
                currentWayPoint = 0;
            }
            targetWayPoint = wayPointList[currentWayPoint];
        }
    }
}