using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockReset : MonoBehaviour {

    Vector3 position, markerPosition;
    Quaternion rotation;
    public GameObject marker, spawn, finishLine;
    float maxScale, minScale;
    int scaleBlock, rescaleBlock;
    public int direction;
    public float markerSpeed;
    public Material growable;
    public Mesh square;

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
        if (position != transform.position && (transform.position.z > finishLine.transform.position.z && direction >= 0))
        {
            marker.transform.localPosition = new Vector3(marker.transform.localPosition.x, marker.transform.localPosition.y, marker.transform.localPosition.z + markerSpeed);
        }
        else if (position != transform.position && (transform.position.z < finishLine.transform.position.z && direction < 0))
        {
            marker.transform.localPosition = new Vector3(marker.transform.localPosition.x, marker.transform.localPosition.y, marker.transform.localPosition.z - markerSpeed);
        }
        if (marker.transform.position.z < finishLine.transform.position.z && transform.position.z > finishLine.transform.position.z && direction >= 0)
        {
            transform.position = position;
            marker.transform.position = markerPosition;
            transform.rotation = rotation;
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }else if (marker.transform.position.z > finishLine.transform.position.z && transform.position.z < finishLine.transform.position.z && direction < 0)
        {
            transform.position = position;
            marker.transform.position = markerPosition;
            transform.rotation = rotation;
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    { 
        if (other.tag == "Teleporter")
        {
            gameObject.GetComponent<Renderer>().material = growable;
            transform.GetComponent<MeshFilter>().mesh = square;
            transform.rotation = new Quaternion(transform.rotation.w, transform.rotation.x, 0, transform.rotation.z);
            transform.position = spawn.transform.position;
            transform.rotation = new Quaternion(transform.rotation.w, transform.rotation.x, 0, transform.rotation.z);
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        if (other.tag == "ColliderSwitch")
        {
            Destroy(other);
            transform.GetComponent<Interactable>().ignore(true);
            Destroy(transform.GetComponent<SphereCollider>());
            transform.gameObject.AddComponent<BoxCollider>();
        }
    }
}
