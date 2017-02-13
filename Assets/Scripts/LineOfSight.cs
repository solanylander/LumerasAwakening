using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineOfSight : MonoBehaviour {

    //Objects variables
    int check;
    int retainSight;
    bool sight;
    public GameObject player;
    Vector3 direction;
    Vector3 position;
    Ray sightRay;
    RaycastHit hit;
    public Transform goal;
    Vector3 lastKnownPosition;
    NavMeshAgent agent;

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.position;
        //Initially set everything to its 0 equivilent
        check = 0;
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

        //If the enemy can currently see the player
        if (sight == true)
        {
            //Amount of frames the enemy still remembers that they saw the player after line of sight is broken
            retainSight = 1;
            //Set the last known position of the player to the players current position
            lastKnownPosition = new Vector3(goal.position.x, goal.position.y, goal.position.z);
        }
        agent.destination = lastKnownPosition;
        checkSight();

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
            if (hit.collider.tag == "Play")
            {
                sight = true;
            }
            else
            {
                sight = false;
            }
        }

    }
}
