using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : MonoBehaviour
{

    public float height;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        other.transform.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, height, other.transform.GetComponent<Rigidbody>().velocity.z);
    }
    void OnTriggerStay(Collider other)
    {
        other.transform.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, height, other.transform.GetComponent<Rigidbody>().velocity.z);
    }
}