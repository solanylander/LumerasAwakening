using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEventTrigger : MonoBehaviour {

    private PrismTest prismTest;
    public GameObject door;
    public int displacement;
    [Range(0.05f, 1.0f)]
    public float displacementSpeed;
    private Vector3 originalPosition;
    private Vector3 endPosition;
    [Range(0, 1)]
    public int triggerOnActive;

	void Start () {
        prismTest = GetComponent<PrismTest>();
        originalPosition = door.transform.position;
        endPosition = new Vector3(originalPosition.x, originalPosition.y + displacement, originalPosition.z);
	}
	
	void FixedUpdate () {
        if (triggerOnActive == 1)
        {
            if (prismTest.beamActive)
            {
                //event to happen when activated
                door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, displacementSpeed);
            }
            else
            {
                door.transform.position = Vector3.MoveTowards(door.transform.position, originalPosition, displacementSpeed);
            }
        } else
        {
            if (!prismTest.beamActive)
            {
                //event to happen when activated
                door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, displacementSpeed);
            }
            else
            {
                door.transform.position = Vector3.MoveTowards(door.transform.position, originalPosition, displacementSpeed);
            }
        }
	}
}
