using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    bool open;
    public float Size;
    public bool grow, locked, extra;
    public GameObject door;
    public Vector3 start;

	// Use this for initialization
	void Start () {
        open = false;
        start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(transform.localPosition.z > -5 && transform.localPosition.y < 2.44 && extra)
        {
            transform.position = start;
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (locked)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }
        if (open && door.transform.localScale.y > Size && !grow)
        {
            door.transform.localScale = new Vector3(door.transform.localScale.x, door.transform.localScale.y - 0.05f, door.transform.localScale.z);
        }else if(open && door.transform.localScale.z < Size && grow)
        {
            door.transform.localScale = new Vector3(door.transform.localScale.x, door.transform.localScale.y, door.transform.localScale.z + 0.05f);
        }else if((open && door.transform.localScale.z >= Size && grow) || (open && door.transform.localScale.y <= Size && !grow))
        {
            Destroy(gameObject);
        }
	}


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Teleporter")
        {
            open = true;
        }
    }
}
