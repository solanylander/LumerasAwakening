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

public class GrabController : MonoBehaviour {
	//Trying to avoid rewriting this when everything gets moved to 3rd person, making this public
	//For 1st - Object w/ camera
	//For 3rd - Actual player avatar
	public GameObject playerCharacter; 
	[Range(1.0f, 10.0f)]
	public float objectHoldOffset =  2.5f;
	public float throwForce = 2.5f;
	
	private GameObject currentTarget;
	private float maxRange;
	private int layerMask = 0;

	private bool currentlyHoldingObject;
	private RaycastHit hit;

	private Renderer currentRenderer;
	private Material defaultMaterial;
	public Material outlineMaterial;
	private PowerController powerScript;

	void Start () {
		currentTarget = transform.gameObject.GetComponent<PowerController>().currentTarget; 
		layerMask = (1 << LayerMask.NameToLayer("Interactable"));
		maxRange = transform.gameObject.GetComponent<PowerController>().maxRange;
		powerScript = transform.gameObject.GetComponent<PowerController>();
	}

	void FixedUpdate () {
		//Temp assign to R - need to remap the joystick controls
		if( Input.GetAxisRaw("Fire1") != 0)
		{
			if (!currentlyHoldingObject)
			{
				//origin of raycast in world coordinates
				Vector3 rayOrigin = playerCharacter.transform.position;

				//TODO: target selection should occur in this controller independently
				//TODO: implement this, check raycast out before trying to do anything to the object
				if (Physics.Raycast(rayOrigin, playerCharacter.transform.forward, out hit, maxRange, layerMask) && hit.rigidbody.gameObject.tag.Contains("Interactable"))
				{
					//On hit to object - select it, and then hold it
					//Interaction with PowerController -- target selection happens in both 
					powerScript.SelectTarget(currentTarget);
					// Call your event function here.
					currentlyHoldingObject = true;
					//TODO: Raycast to object, confirm range and LoS
					HoldObject(currentTarget);
				}
			}
		}
		if (Input.GetAxisRaw("Fire1") == 0) //Specifically == 0 since values range between [-1,1]
		{
			powerScript.ClearTarget(currentTarget);
			//Trigger not in use - don't do anything but leave this code here so I know what the function return means
		}    
	}

	/// <summary>
    /// Hold the object infront of the player character at specified offset 
    /// </summary>
    /// <param name="targetInteractable"></param>
	void HoldObject(GameObject targetInteractable)
	{
		Vector3 playerCenterOfMass = playerCharacter.transform.position;
		Vector3 lookDirectionUnitVector = playerCharacter.transform.forward;
		Vector3 newObjectPosition = playerCenterOfMass + (lookDirectionUnitVector * objectHoldOffset);
		targetInteractable.transform.position = newObjectPosition;
	}

	/// <summary>
    /// Drop the object at the current world position it is being held
    /// </summary>
    /// <param name="targetInteractable"></param>
	void DropObject(GameObject targetInteractable) 
	{
		//TODO: drop object at current location
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
