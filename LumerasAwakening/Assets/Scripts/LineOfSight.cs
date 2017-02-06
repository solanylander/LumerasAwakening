using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {

    //Objects Variables
    int check;
    int retainSight;
    bool sight;
    public GameObject player;
    Vector2 moveTowards;
    Vector3 lastKnownPosition;

    // Use this for initialization
    void Start () {
        //Initially set everything to its 0 equivilent
        check = 0;
        retainSight = 0;
        sight = false;
        lastKnownPosition = transform.position;
        moveTowards = new Vector2(0.0f, 0.0f);
	}
    //FixedUpdate
    void FixedUpdate()
    {
        //Get the normalized direction of the player from the enemy
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();

        //If the enemy can currently see the player
        if(sight == true)
        {
            //Amount of frames the enemy still remembers that they saw the player after line of sight is broken
            retainSight = 10;
            //Set the last known position of the player to the players current position
            lastKnownPosition = player.transform.position;
        }
        //Converted to an int to remove some precision but basically says if (enemies position != players last known position) then true
        if(((int)transform.position.x) != ((int)lastKnownPosition.x) || ((int)transform.position.z) != ((int)lastKnownPosition.z))
        {
            //Get a normalized vector of the direction from the players last know position to the enemies current position
            moveTowards = new Vector2(lastKnownPosition.x - transform.position.x, lastKnownPosition.z - transform.position.z);
            moveTowards.Normalize();
            //move towards the last know position
            transform.position = new Vector3(transform.position.x + (moveTowards.x * 0.1f), transform.position.y, transform.position.z + (moveTowards.y * 0.1f));
        }
        //Every fourth time round pass
        if (check == 4)
        {
            //reset counter to zero
            check = 0;
            //This whole chunk of code says fire an invisible projectile at the player from the enemy.
            //If the projectile hits the player then this means there is line of sight
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
            Vector3 position = player.transform.position;
            go.transform.localScale = scale;
            go.AddComponent<LOSprojectile>();
            go.transform.GetComponent<LOSprojectile>().setDirection(direction);
            go.transform.GetComponent<LOSprojectile>().setPlayerPosition(position);
            go.tag = "Projectile";
            go.GetComponent<MeshRenderer>().enabled = false;
            go.transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z + direction.z);
            go.AddComponent<Rigidbody>(); // Add the rigidbody.
            go.GetComponent<LOSprojectile>().line = this;
        }
        //Every fourth time round on the second count if retainSight is greater than 0 pass
        else if(check == 2 && retainSight > 0)
        {
            //decrement retainSight
            retainSight--;
            //Fire a bullet in the diretion of the players last known position
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
            go.transform.localScale = scale;
            go.AddComponent<LOSprojectile>();
            go.transform.GetComponent<LOSprojectile>().setDirection(direction);
            go.transform.GetComponent<LOSprojectile>().setPlayerPosition(lastKnownPosition);
            go.tag = "Projectile";
            go.GetComponent<MeshRenderer>().enabled = true;
            go.transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z + direction.z);
            go.AddComponent<Rigidbody>(); // Add the rigidbody.
            go.GetComponent<LOSprojectile>().line = this;
            check++;
        }
        else
        {
            check++;
        }
    }

    public void setSight(bool new_sight)
    {
        sight = new_sight;
    }
}
