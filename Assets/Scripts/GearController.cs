using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour {

    //Variables
    public float rotate, doorPosition;
    public bool spinning, final;
    public GameObject door;
    bool colliding, free;
    Vector3 scale;
    int counter, doorCounter, blockCounter;
    

	// Use this for initialization
	void Start () {
        doorPosition = door.transform.position.y;
        scale = transform.localScale;
        colliding = false;
        free = true;
        doorCounter = -1;
        blockCounter = -1;
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
            if(doorCounter > 0)
            {
                doorCounter--;
            }
            if(doorCounter == 0)
            {

                if (door.transform.position.y > doorPosition)
                {
                    door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.1f, door.transform.position.z);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "InteractableXScalableYScalableSpGear" || other.tag == "InteractableXScalableYScalableIGear") && transform.tag != "InteractableXScalableYScalableIGear")
        {
            spinning = true;
            transform.tag = "InteractableXScalableYScalableSpGear";
        }
        colliding = true;
        SphereCollider myCollider = transform.GetComponent<SphereCollider>();
        myCollider.radius = 1.3f;
        if(other.tag == "Wall")
        {
            blockCounter = 10;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if ((other.tag == "InteractableXScalableYScalableSpGear" || other.tag == "InteractableXScalableYScalableIGear") && transform.tag != "InteractableXScalableYScalableIGear")
        {
            spinning = true;
            transform.tag = "InteractableXScalableYScalableSpGear";
        }
        if(other.tag != "InteractableXScalableYScalableIGear" && transform.tag != "InteractableXScalableYScalableIGear")
        {
            if(counter == -1)
            {
                counter = 5;
            }else
            {
                counter--;
                if(counter == 0)
                {
                    spinning = false;
                    transform.tag = "InteractableXScalableYScalableStGear";
                }
            }
        }else
        {
            counter = -1;
        }
        colliding = true;
        SphereCollider myCollider = transform.GetComponent<SphereCollider>();
        myCollider.radius = 1.3f;
        if (other.tag == "Wall")
        {
            blockCounter = 10;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (transform.tag != "InteractableXScalableYScalableIGear")
        {
            spinning = false;
            transform.tag = "InteractableXScalableYScalableStGear";
        }
        colliding = false;
        SphereCollider myCollider = transform.GetComponent<SphereCollider>();
        myCollider.radius = 1.25f;
    }
}
