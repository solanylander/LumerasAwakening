using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour
{

    bool colliding;
    Vector3 scale;
    int counter, stillSpin, flipCheck, wallCounter, lockedCounter, lockedState;
    public float rotate, aligned, puzzle;
    public bool final, sideways, down;
    public GameObject door;
    GameObject prevGear;
    float doorHeight, difference, alignCounter;

    // Use this for initialization
    void Start()
    {
        lockedState = 1;
        lockedCounter = 0;
        difference = 0.0f;
        flipCheck = 0;
        aligned = 1.0f;
        colliding = false;
        scale = transform.localScale;
        counter = -1;
        stillSpin = 0;
        alignCounter = 0.0f;
        wallCounter = 0;
        doorHeight = door.transform.localPosition.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lockedState > 0 && tag != "InteractableXScalableYScalableIGear" && tag != "InteractableXScalableYScalableUGear")
        {
            lockedState--;
        }
        if (lockedCounter > 0)
        {
            lockedCounter--;
        }
        if (tag == "InteractableXScalableYScalableStGear" && flipCheck > 0)
        {
            flipCheck--;
        }
        if(wallCounter > 0)
        {
            wallCounter--;
        }
        if (aligned == 0.0f)
        {
            if (prevGear.GetComponent<GearController>().aligned == 1.0f)
            {
                float prevAngle = Mathf.Atan(Mathf.Abs(transform.localPosition.z - prevGear.transform.localPosition.z)/Mathf.Abs(transform.localPosition.y - prevGear.transform.localPosition.y));

                prevAngle = (prevAngle * 180.0f / Mathf.PI);
                if (transform.localPosition.y - prevGear.transform.localPosition.y < 0)
                {
                    if (transform.localPosition.z - prevGear.transform.localPosition.z > 0)
                    {
                        prevAngle = prevAngle + 180.0f;
                    }
                    else
                    {
                        prevAngle = 180.0f - prevAngle;
                    }
                }
                else
                {
                    if (transform.localPosition.z - prevGear.transform.localPosition.z > 0)
                    {
                        prevAngle = 360.0f - prevAngle;
                    }
                    else
                    {
                        //Do nothing
                    }
                }
                difference = (36.0f + transform.eulerAngles.z - ((prevAngle + 18) + (prevAngle - prevGear.transform.eulerAngles.z) % 36.0f)) % 36.0f;
                if(difference < 0.0f)
                {
                    difference = difference + 36.0f;
                }
                aligned = 2.0f;
            }
        }

        if(aligned == 2.0f && tag == "InteractableXScalableYScalableSpGear")
        {
            if (rotate > 0.0f)
            {
                transform.Rotate(0.0f, 0.0f, -difference);
            }
            else
            {
                transform.Rotate(0.0f, 0.0f, (36.0f - difference));
            }
            aligned = 1.0f;
        }


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
            if(lockedCounter == 0)
            {
                transform.Rotate(0.0f, 0.0f, rotate);
            }
            stillSpin--;
        }
        if(final && transform.tag == "InteractableXScalableYScalableSpGear" && lockedCounter == 0)
        {
            if (sideways && down)
            {
                if (door.transform.localScale.y > 0.1f)
                {
                    door.transform.localScale = new Vector3(door.transform.localScale.x, door.transform.localScale.y - 0.1f, door.transform.localScale.z);
                }
            }
            else if (down)
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
            myCollider.radius = 0.49f;
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
            if (final)
            {

                myCollider.radius = 0.56f;
            }
            else
            {
                myCollider.radius = 0.52f;
            }
            colliding = true;
            if ((other.tag == "InteractableXScalableYScalableSpGear" || other.tag == "InteractableXScalableYScalableIGear") && (transform.tag != "InteractableXScalableYScalableIGear" && transform.tag != "InteractableXScalableYScalableUGear"))
            {
                if (transform.tag == "InteractableXScalableYScalableStGear" && counter != 0 && flipCheck == 0)
                {
                    aligned = 0.0f;
                    prevGear = other.gameObject;
                }
                lockedState = 5;
                if (wallCounter == 0)
                {
                    flipCheck = 5;
                    transform.tag = "InteractableXScalableYScalableSpGear";
                    stillSpin = 12;
                }
            }
            if (other.tag == "Wall")
            {
                if (lockedState > 0)
                {
                    lockedCounter = 10;
                    GameObject[] spinners = GameObject.FindGameObjectsWithTag("InteractableXScalableYScalableSpGear");
                    foreach (GameObject spin in spinners)
                    {
                        if (puzzle == spin.GetComponent<GearController>().puzzle)
                            spin.GetComponent<GearController>().lockedCounter = 10;
                    }
                    GameObject[] initial = GameObject.FindGameObjectsWithTag("InteractableXScalableYScalableIGear");
                    foreach (GameObject init in initial)
                    {
                        if (puzzle == init.GetComponent<GearController>().puzzle)
                            init.GetComponent<GearController>().lockedCounter = 10;
                    }
                }
                wallCounter = 10;
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
