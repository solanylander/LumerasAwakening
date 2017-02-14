using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldController : MonoBehaviour {
	//Trying to avoid rewriting this when everything gets moved to 3rd person, making this public
	//For 1st - Object w/ camera
	//For 3rd - Actual player avatar
	public GameObject playerCharacter; 
	[Range(1.0f, 10.0f)]
	public float objectHoldOffset =  5.0f;
	public float throwForce = 2.5f;
	
	private GameObject localCurrentTarget;
	private float maxRange;
	private int layerMask = 0;
	private bool currentlyHoldingObject;
	private RaycastHit hit;

	private PowerController powerController;

	void Start () {
		layerMask = (1 << LayerMask.NameToLayer("Interactable"));
		powerController = transform.gameObject.GetComponent<PowerController>();
		maxRange = powerController.maxRange; //Have maxRange of obj levitation power same as scaling for now
	}

	void FixedUpdate () {
		localCurrentTarget = powerController.currentTarget; 
		//Temp assign to F - need to remap the joystick controls
		//Simulate releasing the trigger by releasing on key up -- placeholder code for kb controls
		if (Input.GetKeyUp(KeyCode.F))
		{
			if (localCurrentTarget != null) 
			{
				DropObject(localCurrentTarget);
			}
		} else if (Input.GetKey(KeyCode.F)) //(Input.GetAxisRaw("Fire1") != 0) ==Triger Input==
		{
			if (!currentlyHoldingObject)
			{
				//TODO: figure out how to handle objects we want to be fixed in location
				//Allow picking up any object?
				//Set and unset rigidbody.contraints with appropriate bitmasks while holding & when dropped?
				
				Vector3 rayOrigin = playerCharacter.transform.position;
				if (Physics.Raycast(rayOrigin, playerCharacter.transform.forward, out hit, maxRange, layerMask) && hit.rigidbody.gameObject.tag.Contains("Interactable"))
				{
					//On hit to object - select it, and then hold it
					//Interaction with PowerController -- target selection happens in both 
					powerController.SelectTarget(hit.rigidbody.gameObject);
					//TODO: Fix clipping bugs when looking down w/ obj
					HoldObject(hit.rigidbody.gameObject);
					currentlyHoldingObject = true;
				}
			} else {
				HoldObject(localCurrentTarget);
			}
		}
		else if (Input.GetKeyDown(KeyCode.G))//(Input.GetAxisRaw("Fire1") == 0) ==Trigger Input== //Specifically == 0 since values range between [-1,1]
		{
			if (localCurrentTarget != null)
			{
				DropObject(localCurrentTarget);
			}
		}    
	}

	/// <summary>
    /// Hold the object infront of the player character at specified offset 
    /// </summary>
    /// <param name="targetInteractable"></param>
	void HoldObject(GameObject targetInteractable)
	{
		//TODO: Fix rotation of object as player moves camera
		Vector3 playerCenterOfMass = playerCharacter.transform.position;
		Vector3 lookDirectionUnitVector = playerCharacter.transform.forward;
		Vector3 newObjectPosition = playerCenterOfMass + (lookDirectionUnitVector * objectHoldOffset);
		//TODO: instead of setting position, have it move to character smoothly
		targetInteractable.GetComponent<Rigidbody>().MovePosition(newObjectPosition);
	}

	/// <summary>
    /// Drop the object at the current world position it is being held
    /// </summary>
    /// <param name="targetInteractable"></param>
	void DropObject(GameObject targetInteractable) 
	{
		//TODO: drop object at current location
		//This doesn't actually do much, probably just kill it
		powerController.ClearTarget(localCurrentTarget);
		currentlyHoldingObject = false;
	}

	/// <summary>
    /// Throw the object being held with specified force, defined by throwForce
    /// </summary>
    /// <param name="targetInteractable"></param>
	void ThrowObject(GameObject targetInteractable)
	{
		//TODO: throw object with specified force in look direction
	}


}
