using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEventTrigger : MonoBehaviour {

    private PrismTest prismTest;
    public GameObject door;
    private Vector3 originalPosition;
    private Vector3 endPosition;

	void Start () {
        prismTest = GetComponent<PrismTest>();
        originalPosition = door.transform.position;
        endPosition = new Vector3(originalPosition.x, originalPosition.y + 10, originalPosition.z);
	}
	
	void Update () {
		if (prismTest.beamActive)
        {
            //event to happen when activated
            door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, 0.05f);
        } else
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, originalPosition, 0.05f);
        }
	}
}
