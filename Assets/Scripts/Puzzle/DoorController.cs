using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    bool open;
    public float Size;
    public bool grow;
    public GameObject door;

	// Use this for initialization
	void Start () {
        open = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
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
