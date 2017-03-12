using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{

    int stage;

    // Use this for initialization
    void Start()
    {
        stage = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stage == 1)
        {
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, -10.0f, 0.0f));
        }
        else if (stage == 2)
        {
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 60.0f, 300.0f));
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Slide1")
        {
            stage = 1;
        }
        else if (other.tag == "Slide2")
        {
            stage = 2;
        }
        else if (other.tag == "Slide3")
        {
            transform.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 7.0f, 4.9f);
            stage = 0;
        }
    }
}