using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour {

    //Variables
    public float rotate;
    float doorPosition;
    public bool spinning, final;
    public GameObject door;
    bool colliding, free, stay;
    Vector3 scale, connection, blankConnection;
    int counter, doorCounter, blockCounter;
    

	// Use this for initialization
	void Start () {
        doorPosition = door.transform.position.y;
        scale = transform.localScale;
        colliding = false;
        free = true;
        stay = true;
        doorCounter = -1;
        blockCounter = -1;
        blankConnection = new Vector3(0.0f, 0.0f, 0.0f);
        connection = blankConnection;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(blockCounter > 0)
        {
            free = false;
            blockCounter--;
        }
        if (blockCounter == 0)
        {
            blockCounter--;
            free = true;
        }
        if (spinning && free)
        {
            transform.Rotate(0.0f, 0.0f, rotate);
        }
        if (colliding && transform.localScale.x > scale.x)
        {
            transform.localScale = scale;
        }
        else
        {
            scale = transform.localScale;
        }
        if (final && transform.tag == "InteractableXScalableYScalableSpGear")
        {
            doorCounter = -1;
            if (door.transform.position.y < doorPosition + 4.0f)
            {
                door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y + 0.1f, door.transform.position.z);
            }
        }
        if (final && transform.tag == "InteractableXScalableYScalableStGear")
        {
            if(doorCounter == -1)
            {
                doorCounter = 10;
            }
            else if(doorCounter > 0)
            {
                doorCounter--;
            }
            if(doorCounter == 0 && door.transform.position.y > doorPosition)
            {
                door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.1f, door.transform.position.z);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "InteractableXScalableYScalableStGear" && transform.tag != "InteractableXScalableYScalableIGear" && connection == blankConnection)
        {
                transform.tag = "InteractableXScalableYScalableSpGear";
                connection = other.transform.position;
                spinning = true;
                stay = true;
        }
        colliding = true;
        SphereCollider myCollider = transform.GetComponent<SphereCollider>();
        myCollider.radius = 1.3f;
        if(other.tag == "Wall")
        {
            if (transform.tag != "InteractableXScalableYScalableIGear")
            {
                transform.tag = "InteractableXScalableYScalableStGear";
            }
            blockCounter = 10;
        }
    }

    //Debug.Log(connection.x + " " + connection.y + " " + connection.z);
    void OnTriggerStay(Collider other)
    {
        if (connection == blankConnection && transform.tag != "InteractableXScalableYScalableIGear")
        {
            if (stay)
            {
                transform.tag = "InteractableXScalableYScalableStGear";
                spinning = false;
                stay = false;
            }
            else if (other.tag != "InteractableXScalableYScalableStGear")
            {
                transform.tag = "InteractableXScalableYScalableSpGear";
                connection = other.transform.position;
                spinning = true;
                stay = true;
            }
        }
        if (connection == other.transform.position && other.tag == "InteractableXScalableYScalableStGear")
        {
            transform.tag = "InteractableXScalableYScalableStGear";
            spinning = false;
            stay = true;
            connection = blankConnection;
        }
        colliding = true;
        SphereCollider myCollider = transform.GetComponent<SphereCollider>();
        myCollider.radius = 1.3f;
        if (other.tag == "Wall")
        {
            if(transform.tag != "InteractableXScalableYScalableIGear")
            {
                transform.tag = "InteractableXScalableYScalableStGear";
            }
            blockCounter = 10;
        }
    }

    void OnTriggerExit(Collider other)
    {
        colliding = false;
        SphereCollider myCollider = transform.GetComponent<SphereCollider>();
        myCollider.radius = 1.25f;
        if(connection == other.transform.position)
        {
            transform.tag = "InteractableXScalableYScalableStGear";
            spinning = false;
            stay = true;
            connection = blankConnection;
        }
    }
}
