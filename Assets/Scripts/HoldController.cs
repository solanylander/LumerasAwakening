using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~
~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~
	THIS IS NOT FULLY IMPLEMENTED/TESTED YET, DO NOT USE
~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~
~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~-=.=-~
*/

public class HoldController : MonoBehaviour {
	//Trying to avoid rewriting this when everything gets moved to 3rd person, making this public
	//For 1st - Object w/ camera
	//For 3rd - Actual player avatar
	public GameObject playerCharacter; 
	[Range(1.0f, 10.0f)]
	public float objectHoldOffset =  2.5f;
	public float throwForce = 2.5f;
	
	private GameObject localCurrentTarget;
	private float maxRange;
	private int layerMask = 0;

	private bool currentlyHoldingObject;
	private RaycastHit hit;

	private Renderer currentRenderer;
	private Material defaultMaterial;
	public Material outlineMaterial;
	private PowerController powerScript;
	void Start () {
		layerMask = (1 << LayerMask.NameToLayer("Interactable"));
		powerScript = transform.gameObject.GetComponent<PowerController>();
		maxRange = powerScript.maxRange;
	}

	void FixedUpdate () {
		localCurrentTarget = powerScript.currentTarget; 
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
				//origin of raycast in world coordinates
				Vector3 rayOrigin = playerCharacter.transform.position;

				//TODO: figure out how to handle objects we want to be fixed in location
				//Allow picking up any object?
				//Set and unset rigidbody.contraints with appropriate bitmasks while holding & when dropped?

				//TODO: target selection should occur in this controller independently
				//TODO: implement this, check raycast out before trying to do anything to the object
				if (Physics.Raycast(rayOrigin, playerCharacter.transform.forward, out hit, maxRange, layerMask) && hit.rigidbody.gameObject.tag.Contains("Interactable"))
				{
					//On hit to object - select it, and then hold it
					//Interaction with PowerController -- target selection happens in both 
					powerScript.SelectTarget(hit.rigidbody.gameObject);
					//TODO: Fix clipping bugs
					HoldObject(hit.rigidbody.gameObject);
					// Call your event function here.
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
		//TODO: instead of setting position, have it move to character smoothyl
		targetInteractable.GetComponent<Rigidbody>().MovePosition(newObjectPosition);
		//targetInteractable.GetComponent<Rigidbody>().useGravity = false;
	}

	/// <summary>
    /// Drop the object at the current world position it is being held
    /// </summary>
    /// <param name="targetInteractable"></param>
	void DropObject(GameObject targetInteractable) 
	{
		//targetInteractable.GetComponent<Rigidbody>().useGravity = true;
		//TODO: drop object at current location
		powerScript.ClearTarget(localCurrentTarget);
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
