using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineOfSight : MonoBehaviour {

    //Objects variables
    int check, checkpointCounter, checkX, checkZ;
    int retainSight;
    bool sight;
    public GameObject player;
    Vector3 direction;
    Vector3 position;
    Ray sightRay;
    RaycastHit hit;
    Vector3 lastKnownPosition;
    NavMeshAgent agent;
    float angle, comparisonAngle;

    public float fov;
    public GameObject checkpoint1, checkpoint2, checkpoint3, checkpoint4, checkpoint5;

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.position;
        //Initially set everything to its 0 equivilent
        check = 0;
        angle = 0;
        checkpointCounter = 0;
        retainSight = 0;
        sight = false;
        lastKnownPosition = transform.position;
        direction = new Vector3(0.0f, 0.0f, 0.0f);
        position = new Vector3(0.0f, 0.0f, 0.0f);
    }
    //FixedUpdate
    void FixedUpdate()
    {
        //Get the normalized direction of the player from the enemy
        direction = player.transform.position - transform.position;
        direction.Normalize();
        if(direction.x >= 0)
        {
                angle = (90.0f - (float)(Math.Atan(direction.z / direction.x) / Math.PI) * 180.0f);
        }else
        {
                angle = (270.0f - (float)(Math.Atan(direction.z / direction.x) / Math.PI) * 180.0f);
        }
        comparisonAngle = (transform.rotation.y) * 180.0f;
        if(comparisonAngle < 0.0f)
        {
            comparisonAngle += 360.0f;
        }
        checkSight();

        //If the enemy can currently see the player
        if (sight == true)
        {
            //Amount of frames the enemy still remembers that they saw the player after line of sight is broken
            retainSight = 3;
            //Set the last known position of the player to the players current position
            lastKnownPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            agent.destination = lastKnownPosition;
        }
        //Debug.Log(retainSight);
        if (((int)agent.transform.position.x) == ((int)lastKnownPosition.x) && ((int)agent.transform.position.z) == ((int)lastKnownPosition.z)){
            retainSight = -1;
        }
        if (retainSight == -1)
        { 
            switch (checkpointCounter)
            {
                case 0:
                    agent.destination = checkpoint1.transform.position;
                    checkX = (int) checkpoint1.transform.position.x;
                    checkZ = (int)checkpoint1.transform.position.z;
                    break;
                case 1:
                    agent.destination = checkpoint2.transform.position;
                    checkX = (int)checkpoint2.transform.position.x;
                    checkZ = (int)checkpoint2.transform.position.z;
                    break;
                case 2:
                    agent.destination = checkpoint3.transform.position;
                    checkX = (int)checkpoint3.transform.position.x;
                    checkZ = (int)checkpoint3.transform.position.z;
                    break;
                case 3:
                    agent.destination = checkpoint4.transform.position;
                    checkX = (int)checkpoint4.transform.position.x;
                    checkZ = (int)checkpoint4.transform.position.z;
                    break;
                case 4:
                    agent.destination = checkpoint5.transform.position;
                    checkX = (int)checkpoint5.transform.position.x;
                    checkZ = (int)checkpoint5.transform.position.z;
                    break;
            }
            //Debug.Log(((int)transform.position.x) + " " + checkX + " " + ((int)transform.position.z) + " " + checkZ);
            if (((int)agent.transform.position.x) == checkX && ((int)agent.transform.position.z) == checkZ)
            {
                checkpointCounter++;
                if (checkpointCounter == 5)
                {
                    checkpointCounter = 0;
                }
            }
        }
        //Every fourth time round if retainSight is greater than 0 fire a bullet
        if (check == 4 && retainSight > 0)
        {
            //decrement retainSight
            retainSight--;
            fireBullet();
        }
        check++;
        if(check == 5)
        {
            check = 0;
        }
    }

    void fireBullet()
    {
        //Fire a bullet in the diretion of the players last known position
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
        go.transform.localScale = scale;
        go.AddComponent<Projectile>();
        go.transform.GetComponent<Projectile>().setDirection(direction);
        go.transform.GetComponent<Projectile>().setPlayerPosition(position);
        go.tag = "Projectile";
        go.GetComponent<MeshRenderer>().enabled = true;
        go.transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z + direction.z);
        go.AddComponent<Rigidbody>(); // Add the rigidbody
    }

    void checkSight()
    {
        position = transform.position;
        sightRay = new Ray(position, direction);
        if (Physics.Raycast(sightRay, out hit))
        {
            //Debug.Log(hit.collider.tag);
            //Debug.DrawRay(position, direction, Color.green);
            if (hit.collider.tag == "Player")
            {
                if((comparisonAngle + fov >= angle && comparisonAngle - fov <= angle) || (comparisonAngle + fov + 360.0f >= angle && comparisonAngle - fov +360.0f <= angle) || (comparisonAngle + fov - 360.0f >= angle && comparisonAngle - fov - 360.0f <= angle))
                {
                    sight = true;
                }else
                {
                    sight = false;
                }
            }
            else
            {
                sight = false;
            }
        }

    }
}
