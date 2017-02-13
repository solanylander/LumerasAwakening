using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour {
	
	//Trying to avoid rewriting this when everything gets moved to 3rd person, making this public
	//For 1st - Object w/ camera
	//For 3rd - Actual player avatar
	public GameObject playerCharacter; 

	[Range(1.0f, 10.0f)]
	public float objectHoldOffset =  2.5f;
	
	private GameObject currentTarget;
	private bool currentlyHoldingObject;

	void Start () {
		currentTarget = gameObject.GetComponent<PowerController>().currentTarget; 
	}

	void FixedUpdate () {
		//Temp assign to R - need to remap the joystick controls
		if( Input.GetAxisRaw("Fire1") != 0)
		{
			if (!currentlyHoldingObject)
			{
				// Call your event function here.
				currentlyHoldingObject = true;
				//TODO: Raycast to object, confirm range and LoS
				HoldObject(currentTarget);
			}
		}
		if (Input.GetAxisRaw("Fire1") == 0) //Specifically == 0 since values range between [-1,1]
		{
			//Trigger not in use - don't do anything but leave this code here so I know what the function return means
		}    
	}

	//TODO: add xml comments in vs studio later
	void HoldObject(GameObject targetInteractable)
	{
		Vector3 playerCenterOfMass = playerCharacter.transform.position;
		Vector3 lookDirectionUnitVector = playerCharacter.transform.forward;
		Vector3 newObjectPosition = playerCenterOfMass + (lookDirectionUnitVector * objectHoldOffset);
		//targetInteractable.transform.TODO
		//
	}
}
