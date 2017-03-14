using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour
{

    //Variables
    public float rotate, height;
    float doorPosition;
    public bool spinning, final, sideways;
    public GameObject door;
    bool colliding, free, stay, wasSpinning, achieved;
    Vector3 scale, connection, blankConnection;
    int counter, doorCounter, blockCounter, triggers;


    // Use this for initialization
    void Start()
    {
        doorPosition = door.transform.position.y;
        scale = transform.localScale;
        colliding = false;
        free = true;
        stay = true;
        wasSpinning = true;
        achieved = false;
        doorCounter = -1;
        blockCounter = -1;
        blankConnection = new Vector3(0.0f, 0.0f, 0.0f);
        connection = blankConnection;
        triggers = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (blockCounter > 0)
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
            if (sideways == false)
            {
                if (door.transform.position.y < doorPosition + height && height > 0)
                {
                    door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y + 0.1f, door.transform.position.z);
                }
                else if (door.transform.position.y > doorPosition + height && height < 0)
                {
                    door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.1f, door.transform.position.z);
                }
            }else
            {
                if (door.transform.localScale.z >= 0.18f)
                {
                    door.transform.localScale = new Vector3(door.transform.localScale.x, door.transform.localScale.y, door.transform.localScale.z - 0.02f);
                }
            }
        }
        if (final && transform.tag == "InteractableXScalableYScalableStGear")
        {
            if (doorCounter == -1)
            {
                doorCounter = 10;
            }
            else if (doorCounter > 0)
            {
                doorCounter--;
            }
            if (sideways == false)
            {
                if (doorCounter == 0 && door.transform.position.y > doorPosition && height > 0)
                {
                    door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.1f, door.transform.position.z);
                }
                else if (doorCounter == 0 && door.transform.position.y < doorPosition && height < 0)
                {
                    door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y + 0.1f, door.transform.position.z);
                }
            }else
            {
                if (door.transform.localScale.z <= 2.8f)
                {
                    door.transform.localScale = new Vector3(door.transform.localScale.x, door.transform.localScale.y, door.transform.localScale.z + 0.02f);
                }
            }
        }
        if (spinning == false && wasSpinning == true)
        {
            transform.GetComponent<Interactable>().shrink();
            wasSpinning = false;
        }
        if (triggers == 0 && (transform.tag == "InteractableXScalableYScalableStGear" || transform.tag == "InteractableXScalableYScalableSpGear"))
        {
            transform.tag = "InteractableXScalableYScalableStGear";
            spinning = false;
            stay = true;
            connection = blankConnection;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Untagged")
        {
            triggers++;
            if (other.tag != "InteractableXScalableYScalableStGear" && other.tag != "InteractableXScalableYScalableUGear" && transform.tag != "InteractableXScalableYScalableIGear" && transform.tag != "InteractableXScalableYScalableUGear" && connection == blankConnection)
            {
                transform.tag = "InteractableXScalableYScalableSpGear";
                connection = other.transform.position;
                spinning = true;
                wasSpinning = true;
                stay = true;
            }
            colliding = true;
            SphereCollider myCollider = transform.GetComponent<SphereCollider>();
            myCollider.radius = 1.3f;
            if (other.tag == "Wall")
            {
                if (transform.tag != "InteractableXScalableYScalableIGear" && transform.tag != "InteractableXScalableYScalableUGear" && transform.tag != "InteractableXScalableYScalableUGear")
                {
                    transform.tag = "InteractableXScalableYScalableStGear";
                }
                else
                {
                    transform.tag = "InteractableXScalableYScalableUGear";
                }
            }
        }
    }

    //Debug.Log(connection.x + " " + connection.y + " " + connection.z);
    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Untagged")
        {
            if (connection == blankConnection && transform.tag != "InteractableXScalableYScalableIGear" && transform.tag != "InteractableXScalableYScalableUGear")
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
                    wasSpinning = true;
                    stay = true;
                }
            }
            if (connection == other.transform.position && (other.tag == "InteractableXScalableYScalableStGear" || other.tag == "InteractableXScalableYScalableUGear"))
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
                if (transform.tag != "InteractableXScalableYScalableIGear" && transform.tag != "InteractableXScalableYScalableUGear")
                {
                    transform.tag = "InteractableXScalableYScalableStGear";
                }
                else
                {
                    transform.tag = "InteractableXScalableYScalableUGear";
                }
                blockCounter = 10;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Untagged")
        {
            triggers--;
            colliding = false;
            SphereCollider myCollider = transform.GetComponent<SphereCollider>();
            myCollider.radius = 1.25f;
            if (connection == other.transform.position)
            {
                transform.tag = "InteractableXScalableYScalableStGear";
                spinning = false;
                stay = true;
                connection = blankConnection;
            }
            if (other.tag == "Wall")
            {
                if (transform.tag == "InteractableXScalableYScalableUGear")
                {
                    transform.tag = "InteractableXScalableYScalableIGear";
                }
            }
        }
    }
}
