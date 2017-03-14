using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockReset : MonoBehaviour {

    Vector3 position, markerPosition;
    Quaternion rotation;
    public GameObject marker, spawn, finishLine,  blocker;
    float maxScale, minScale;
    int scaleBlock, rescaleBlock, counter;
    public int direction;
    public float markerSpeed;
    public Material growable;
    public Mesh square;
    bool grow, shrink;
    public bool thrown;
   

    // Use this for initialization
    void Start () {
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        markerPosition = new Vector3(marker.transform.position.x, marker.transform.position.y, marker.transform.position.z);
        rotation = new Quaternion(transform.rotation.w,  transform.rotation.x, transform.rotation.y, transform.rotation.z);
        grow = false;
        shrink = false;
        scaleBlock = 5;
        rescaleBlock = 5;
        counter = -1;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (grow && transform.localScale.x < 7.0f)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.07f, transform.localScale.y + 0.07f, transform.localScale.z + 0.07f);
        }
        if (shrink && blocker.transform.localScale.z > 0.1f)
        {
            blocker.transform.localScale = new Vector3(blocker.transform.localScale.x, blocker.transform.localScale.y, blocker.transform.localScale.z - 0.07f);
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
        if (transform.position.z < finishLine.transform.position.z && transform.GetComponent<Rigidbody>().velocity.z < 0 && direction == -1 && counter == -1)
        {
            counter = 15;
        }
        if(counter >= 0)
        {
            if (counter == 0)
            {
                transform.position = position;
                marker.transform.position = markerPosition;
                transform.rotation = rotation;
                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
            counter--;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Teleporter")
        {
            gameObject.GetComponent<Renderer>().material = growable;
            if (thrown)
            {
                transform.GetComponent<MeshFilter>().mesh = square;
            }else
            {
                Destroy(transform.GetComponent<MeshFilter>().mesh);
            }
            transform.rotation = new Quaternion(transform.rotation.w, transform.rotation.x, 0, transform.rotation.z);
            transform.position = spawn.transform.position;
            transform.rotation = new Quaternion(transform.rotation.w, transform.rotation.x, 0, transform.rotation.z);
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            shrink = true;
        }

        if (other.tag == "ColliderSwitch")
        {
            Destroy(other);
            Destroy(transform.GetComponent<SphereCollider>());
            Destroy(transform.GetComponent<SphereCollider>());
            transform.gameObject.AddComponent<BoxCollider>();
            grow = true;
        }
    }
}
