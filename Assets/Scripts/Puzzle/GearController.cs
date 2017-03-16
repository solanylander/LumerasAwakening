using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour
{

    bool colliding;
    Vector3 scale;
    int counter, stillSpin;
    public float rotate;
    public bool final, sideways, down;
    public GameObject door;
    float doorHeight;

    // Use this for initialization
    void Start()
    {
        colliding = false;
        scale = transform.localScale;
        counter = -1;
        stillSpin = 0;
        doorHeight = door.transform.localPosition.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter--;
        if(counter == 0)
        {
            GameObject[] spinners = GameObject.FindGameObjectsWithTag("InteractableXScalableYScalableSpGear");
            foreach (GameObject spin in spinners)
            {
                spin.tag = "InteractableXScalableYScalableStGear";
            }
        }
        if(transform.tag == "InteractableXScalableYScalableIGear" || transform.tag == "InteractableXScalableYScalableSpGear" || stillSpin > 0)
        {
            transform.Rotate(0.0f, 0.0f, -rotate);
            stillSpin--;
        }
        if(final && transform.tag == "InteractableXScalableYScalableSpGear")
        {
            if (down)
            {
                if (door.transform.localPosition.y > doorHeight - 9.9f)
                {
                    door.transform.localPosition = new Vector3(door.transform.localPosition.x, door.transform.localPosition.y - 0.07f, door.transform.localPosition.z);
                }
            }
            else if (sideways)
            {
                if (door.transform.localScale.z > 0.2)
                {
                    door.transform.localScale = new Vector3(door.transform.localScale.x, door.transform.localScale.y, door.transform.localScale.z - 0.015f);
                }
            }
            else
            {
                if (door.transform.localScale.y < -0.2)
                {
                    door.transform.localScale = new Vector3(door.transform.localScale.x, door.transform.localScale.y + 0.1f, door.transform.localScale.z);
                }
            }
        }
        if(colliding && transform.localScale.x > scale.x)
        {
            transform.localScale = scale;
        }else
        {
            scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Check(other);
    }

    //Debug.Log(connection.x + " " + connection.y + " " + connection.z);
    void OnTriggerStay(Collider other)
    {
        Check(other);
    }

    void OnTriggerExit(Collider other)
    {
        colliding = false;
        if (other.tag != "Untagged")
        {
            SphereCollider myCollider = transform.GetComponent<SphereCollider>();
            myCollider.radius = 1.25f;
            counter = 2;
        }
        if (other.tag == "Wall" && transform.tag == "InteractableXScalableYScalableUGear")
        {
            transform.tag = "InteractableXScalableYScalableIGear";
        }
    }

    void Check(Collider other)
    {
        if (other.tag != "Untagged")
        {
            SphereCollider myCollider = transform.GetComponent<SphereCollider>();
            myCollider.radius = 1.3f;
            colliding = true;
            if ((other.tag == "InteractableXScalableYScalableSpGear" || other.tag == "InteractableXScalableYScalableIGear") && (transform.tag != "InteractableXScalableYScalableIGear" && transform.tag != "InteractableXScalableYScalableUGear"))
            {
                transform.tag = "InteractableXScalableYScalableSpGear";
                stillSpin = 12;
            }
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
            }
        }
    }
}
