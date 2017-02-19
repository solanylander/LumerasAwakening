﻿using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof (LineRenderer))]
public class PowerController : MonoBehaviour
{
    [RangeAttribute(0,1)]
    public int targetLockOn = 1;
    [RangeAttribute(0, 1)]
    public int softLockOn = 0;
    [RangeAttribute(0, 1)]
    public int lazerBeams = 0;
    [RangeAttribute(0, 1)]
    public int freezeWhileScaling = 1;
    [Range(5.0f, 100f)]
    public float maxPowerRange = 40f;
    //public Transform lineOrigin;
    //public GameObject tracerEffect; - Particles
    private bool getNewTarget;

    private Vector3 rayOrigin;
    private LineRenderer tracerLine;
    private Camera playerCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.05f);
    private RaycastHit hit; //holds info about anything hit by ray casted
    //private float hitForce = 250f; //for debugging
    private TargetingController targetingController;
    private int layerMask = 0;

    //Scaling Variables
    [Range(0.01f, 0.1f)]
    public float powerScalar = 0.015f; //Scaling Rate
    [Range(0.0f, 1.0f)]
    public float massScalar = 0.025f; //Mass Scaling Rate
    [Range(0.0f, 1f)]
    public float scaleDelay = 0.35f;
    private float scaleStart;

    void Start()
    {
        //Note: script should be in a child to the player character's camera object
        playerCam = GetComponentInParent<Camera>();
        tracerLine = GetComponentInChildren<LineRenderer>();
        layerMask = (1 << LayerMask.NameToLayer("Interactable")); //Raycast bit mask by shifting index of 'Interactable' layer
        targetingController = transform.gameObject.GetComponent<TargetingController>();
        getNewTarget = true;
    }

    void FixedUpdate()
    {
        switch (targetLockOn)
        {
            case 0:
                if(!(Input.GetButton("Fire1") | Input.GetButton("Fire2"))) 
                {
                    if (targetingController.currentTarget != null)
                    {
                        targetingController.ClearTarget(targetingController.currentTarget);
                        //Bug when power held down, button up on mouse isn't registering
                    }
                    getNewTarget = true;
                }
                break;
        }
        
        //Scaling object when 'shot'
        if ( (Input.GetButton("Fire1") | Input.GetButton("Fire2")) && Time.time > scaleStart )  //TODO: Gamepad mappings 
        {
            //Renders a line, add in spell effect when/if ready, remove for now b/c Ugly
            //StartCoroutine(ShotEffect());

            /* translates where the point is in the viewport to the world coordinate system
            top left = 0, top right = 1, bot left = 1
            translate middle of viewport in x and y to point in world coordinates */
            Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            //Set LineRenderer position to public origin transform
            //tracerLine.SetPosition(0, this.lineOrigin.position); b/c Ugly

            //Ray registers a hit with an Interactable object
            if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, maxPowerRange, layerMask) && hit.rigidbody.gameObject.tag.Contains("Interactable"))
            {
                //tracerLine.SetPosition(1, hit.point); b/c Ugly
                if (hit.rigidbody != null)
                {
                    //For debugging purposes: hit obj @ normal to surface hit
                    //hit.rigidbody.AddForce(-hit.normal * hitForce);

                    //Selecting/Lock on to new target
                    if (targetingController.currentTarget != hit.rigidbody.gameObject && getNewTarget)
                    {
                        targetingController.SelectTarget(hit.rigidbody.gameObject);
                        scaleStart = Time.time + scaleDelay;
                        if (lazerBeams == 0)
                        {
                            //require a separate button press for interaction with a different object
                            getNewTarget = false;
                        }
                    } else if (Input.GetButton("Fire1") && targetingController.currentTarget != null)
                    {
                        ScaleObject(targetingController.currentTarget, 1 + powerScalar);
                    } else if (Input.GetButton("Fire2") && targetingController.currentTarget != null)
                    {
                        ScaleObject(targetingController.currentTarget, 1 - powerScalar);
                    }     
                }
            }
            else if (targetingController.currentTarget != null && softLockOn == 1)
            {
                //Just render the line of length maxPowerRange
                //tracerLine.SetPosition(1, lineOrigin + (playerCam.transform.forward * maxPowerRange)); b/c Ugly
                if (Input.GetButton("Fire1"))
                {
                    ScaleObject(targetingController.currentTarget, 1 + powerScalar);
                }
                else if (Input.GetButton("Fire2"))
                {
                    ScaleObject(targetingController.currentTarget, 1 - powerScalar);
                }
            } else
            {
                if (targetingController.currentTarget != null)
                {
                    targetingController.ClearTarget(targetingController.currentTarget);
                }
            }
        }
    }

    /// <summary>
    /// Scale the target gameObject based on given scalar rate in the dimension specified by the object tag.
    /// </summary>
    /// <remarks>
    /// Scaling is a muliplication operation to create some input acceleration (smaller: more granular, larger: quicker scaling)
    /// Anchored Interactables are children of invisible 'anchor' objects which redefine the pivot point for scaling.
    /// Objects can be tagged with any combination/order of Interactable, Anchored, [X|Y|Z]Scalable concatenated 
    /// TODO: Move decay & scaling into Interactable & specify individual XYZ scale limits & decayRates
    /// </remarks>
    /// <param name="targetInteractable"></param>
    /// <param name="scaleRate"></param>
    private void ScaleObject(GameObject targetInteractable, float scaleRate)
    {
        Rigidbody targetRB = targetInteractable.GetComponent<Rigidbody>();
        targetRB.detectCollisions = false;
        RigidbodyConstraints originalConstraints = targetRB.constraints;
        //Freeze object while scaling
        if (freezeWhileScaling == 1)
        {
            targetRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
        }

        Interactable interactionController = targetInteractable.GetComponent<Interactable>();     
        float limitedScale;
        Vector3 curScale;
        //Anchored objects are the children of anchor objects which are used to 'change' the pivot point of the object
        if (targetInteractable.tag.Contains("Anchored"))
        {
            curScale = targetInteractable.GetComponentInParent<Transform>().parent.gameObject.transform.localScale;
        } else
        {
            curScale = targetInteractable.transform.localScale;
        }
        //TODO: update MASS --UpdateMass()-- based on limitedScale - update only if scale is changed & not @ limit, max/min Mass interaction? tuning this is weird/tricky   
        if (targetInteractable.tag.Contains("XScalable"))
        {
            limitedScale = Math.Max(Math.Min(curScale.x * scaleRate, interactionController.maxScale), interactionController.minScale);
            if (targetInteractable.tag.Contains("Anchored"))
            {
                targetInteractable.GetComponentInParent<Transform>().parent.gameObject.transform.localScale = new Vector3(limitedScale, curScale.y, curScale.z);
            } else
            {
                targetInteractable.transform.localScale = new Vector3(limitedScale, curScale.y, curScale.z);
            }
            curScale = targetInteractable.transform.localScale;
        }
        if (targetInteractable.tag.Contains("YScalable"))
        {
            limitedScale = Math.Max(Math.Min(curScale.y * scaleRate, interactionController.maxScale), interactionController.minScale);
            if (targetInteractable.tag.Contains("Anchored"))
            {
                targetInteractable.GetComponentInParent<Transform>().parent.gameObject.transform.localScale = new Vector3(curScale.x, limitedScale, curScale.z);
            } else
            {
                targetInteractable.transform.localScale = new Vector3(curScale.x, limitedScale, curScale.z);
            }
            curScale = targetInteractable.transform.localScale;
        }
        if (targetInteractable.tag.Contains("ZScalable"))
        {
            limitedScale = Math.Max(Math.Min(curScale.z * scaleRate, interactionController.maxScale), interactionController.minScale);
            if (targetInteractable.tag.Contains("Anchored"))
            {
                targetInteractable.GetComponentInParent<Transform>().parent.gameObject.transform.localScale = new Vector3(curScale.x, curScale.y, limitedScale);
            } else
            {
                targetInteractable.transform.localScale = new Vector3(curScale.x, curScale.y, limitedScale);
            }
            curScale = targetInteractable.transform.localScale;
        } 
        if (targetInteractable.tag.ToString().Equals("Interactable"))
        {
            //TODO: fix this, it's terribly terribly broken do not use this, instead use Interactable[X|Y|Z]Scalable - any combination of the 3
            limitedScale = Math.Max(Math.Min(Math.Max(Math.Max(curScale.x,curScale.y),curScale.z) * scaleRate, interactionController.maxScale), interactionController.minScale);
            targetInteractable.transform.localScale = new Vector3(curScale.x * scaleRate, curScale.y * scaleRate, curScale.z * scaleRate);
        }

        //Set constraints back to originals if RB Interactable, re-enable collisions
        targetRB.detectCollisions = true;
        targetRB.constraints = originalConstraints;
        float massRate = 1.0f;
        massRate = (scaleRate > 1.0f) ? (massRate + massScalar) : ((scaleRate < 1.0f) ? (massRate - massScalar): massRate);
        UpdateMass(targetInteractable, massRate, "placeHolder");
    }

    /// <summary>
    /// Update object mass proportionally to it's scale (?and potentially material type?)
    /// </summary>
    /// <remarks>
    /// TODO: figure out if there are going to be different material types whose mass will scale differently depending on type
    /// Note: Unity mass units are kg
    /// </remarks>
    /// <param name="targetInteractable"></param>
    /// <param name="massRate"></param>
    /// <param name="objectMaterial"></param>
    private void UpdateMass(GameObject targetInteractable, float massRate, string objectMaterial)
    {
        Interactable interactionController = targetInteractable.GetComponent<Interactable>();    
        float currentTargetMass = targetInteractable.GetComponent<Rigidbody>().mass;
        float scalingDimensions = targetInteractable.tag.Equals("Interactable") ? 1.0f : float.Epsilon;
        scalingDimensions = targetInteractable.tag.Contains("XScalable") ? scalingDimensions + 1 : scalingDimensions;
        scalingDimensions = targetInteractable.tag.Contains("YScalable") ? scalingDimensions + 1 : scalingDimensions;
        scalingDimensions = targetInteractable.tag.Contains("ZScalable") ? scalingDimensions + 1 : scalingDimensions;
        //If scaling in only one dimension, scale by cubic root of the given massRate 
        massRate = (float)Math.Pow(massRate, (1.0f/scalingDimensions)); //Pls no floating point errors
        
        //Update mass given limiters [min|max]Masss
        targetInteractable.GetComponent<Rigidbody>().mass = Math.Max(interactionController.minMass, Math.Min(currentTargetMass * massRate, interactionController.maxMass));
    }

    /// <summary>
    /// //coroutine (think separate thread) to render tracer effect for shotDuration seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotEffect()
    {
        //Show tracer line for shotDuration seconds
        //Instantiate(tracerEffect, transform.position, Quaternion.identity); -- particles
        tracerLine.enabled = true;
        yield return shotDuration;
        tracerLine.enabled = false;
    }
}

