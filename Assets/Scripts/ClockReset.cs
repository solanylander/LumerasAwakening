using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockReset : MonoBehaviour {

    Vector3 position, markerPosition;
    Quaternion rotation;
    public GameObject marker;
    float maxScale, minScale;
    int scaleBlock, rescaleBlock;
    public float markerSpeed;
    public Material growable;

    // Use this for initialization
    void Start () {
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        markerPosition = new Vector3(marker.transform.position.x, marker.transform.position.y, marker.transform.position.z);
        rotation = new Quaternion(transform.rotation.w,  transform.rotation.x, transform.rotation.y, transform.rotation.z);
        scaleBlock = 5;
        rescaleBlock = 5;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (scaleBlock > 0)
        {
            transform.GetComponent<Interactable>().ignore(false);
            scaleBlock--;
        }
        if (position != transform.position && transform.localPosition.z > -7.35f)
        {
            marker.transform.position = new Vector3(marker.transform.position.x, marker.transform.position.y, marker.transform.position.z - markerSpeed);
        }
        if(marker.transform.localPosition.z < 0.43f && transform.localPosition.z > -7.35f)
        {
            transform.position = position;
            marker.transform.position = markerPosition;
            transform.rotation = rotation;
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (rescaleBlock > 0 && transform.localPosition.y < -7.0f)
        {
            transform.GetComponent<Interactable>().ignore(true);
            rescaleBlock--;
            gameObject.GetComponent<Renderer>().material = growable;
        }
    }
}
