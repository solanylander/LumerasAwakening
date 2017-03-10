using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePlayerChild : MonoBehaviour {

    //oh god, make the player a child of this object on trigger to smooth out jitters 
    // Use this for initialization
    private GameObject player;
    public bool active;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (gameObject.GetComponentInChildren<Renderer>().bounds.min.x <= player.transform.position.x  && player.transform.position.x <= gameObject.GetComponentInChildren<Renderer>().bounds.max.x && gameObject.GetComponentInChildren<Renderer>().bounds.min.z <= player.transform.position.z && player.transform.position.z <= gameObject.GetComponentInChildren<Renderer>().bounds.max.z)
        {
            active = true;
        } else
        {
            active = false;
        }
		if (active)
        {
            player.transform.parent = gameObject.transform;
        } else
        {
            player.transform.parent = null;
        }
	}
}
