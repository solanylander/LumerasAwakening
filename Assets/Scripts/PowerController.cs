﻿using System;
using System.Collections;
using UnityEngine;

//[RequireComponent(typeof (LineRenderer))]
public class PowerController : MonoBehaviour
{
    //Raycast Variables
    [Range(5.0f, 40f)]
    public float maxPowerRange = 25f;
    //public Transform lineOrigin;
    //public GameObject tracerEffect; - Particles

    private Vector3 rayOrigin;
    private LineRenderer tracerLine;
    private Camera playerCam;
    private float nextFire;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.05f);
    private RaycastHit hit; //holds info about anything hit by ray casted
    //private float hitForce = 250f; //for debugging

    [RangeAttribute(0,1)]
    public int targetLockOn = 1;
    private TargetingController targetingController;
    private int layerMask = 0;

    //Scaling Variables
    [Range(0.01f, 0.1f)]
    public float powerScalar = 0.015f; //Scaling Rate
    [Range(10.0f, 100.0f)]
    public float maxScale = 50.0f; //Growth Limiter TODO: Set on an PER OBJECT basis - finer control
    [Range(0.05f, 1.0f)]
    public float minScale = 0.1f; //Shrinkage Limiter TODO: Set on an PER OBJECT basis - finer control
    //[Range(0.0f, 1.0f)]
    public float massScalar = 0.025f; //Mass Scaling Rate

    void Start()
    {
        //Note: script should be in a child to the player character's camera object
        playerCam = GetComponentInParent<Camera>();
        tracerLine = GetComponentInChildren<LineRenderer>();
        layerMask = (1 << LayerMask.NameToLayer("Interactable")); //Raycast bit mask by shifting index of 'Interactable' layer
        //For prototyping
    }

    void FixedUpdate()
    {
        //Scaling object when 'shot'
        if ( (Input.GetButton("Fire1") | Input.GetButton("Fire2"))) //TODO: Gamepad mappings 
        {
            //nextFire = Time.time + fireRate; 

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
                    if (targetingController.currentTarget != hit.rigidbody.gameObject)
                    {
                        targetingController.SelectTarget(hit.rigidbody.gameObject);
                    } else if (Input.GetButton("Fire1"))
                    {
                        ScaleObject(hit.rigidbody.gameObject, 1 + powerScalar);
                        //NOTE: Constantly selecting/deselecting targets causes flickers with current selected target indication method
                        switch (targetLockOn)
                        {
                            case 0:
                                targetingController.ClearTarget(hit.rigidbody.gameObject);
                                break;
                        }
                    } else if (Input.GetButton("Fire2"))
                    {
                        ScaleObject(hit.rigidbody.gameObject, 1 - powerScalar);
                        switch (targetLockOn)
                        {
                            case 0:
                                targetingController.ClearTarget(hit.rigidbody.gameObject);
                                break;
                        }
                    }     
                }
            }
            else if (targetingController.currentTarget != null)
            {
                //Just render the line of length maxPowerRange
                //tracerLine.SetPosition(1, lineOrigin + (playerCam.transform.forward * maxPowerRange)); b/c Ugly
                if (Input.GetButton("Fire1"))
                {
                    ScaleObject(targetingController.currentTarget, 1 + powerScalar);
                    switch (targetLockOn)
                    {
                        case 0:
                            targetingController.ClearTarget(targetingController.currentTarget.gameObject);
                            break;
                    }
                }
                else if (Input.GetButton("Fire2"))
                {
                    ScaleObject(targetingController.currentTarget, 1 - powerScalar);
                    switch (targetLockOn)
                    {
                        case 0:
                            targetingController.ClearTarget(targetingController.currentTarget.gameObject);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Scale the target gameObject based on given scalar rate in the dimension specified by the object tag.
    /// Scaling is a muliplication operation to create some input acceleration (smaller: more granular, larger: quicker scaling)
    /// Anchored Interactables are children of invisible 'anchor' objects which redefine the pivot point for scaling.
    /// </summary>
    /// <param name="targetInteractable"></param>
    /// <param name="scaleRate"></param>
    private void ScaleObject(GameObject targetInteractable, float scaleRate)
    {
        //TODO: Update scale limiter to be based on targetInteractable parameters
        //targetInteractable.getComponent<Interactable>.[min|max][Scale|Mass]
        
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
        //TODO: update mass based on limitedScale - update only if scale is changed & not @ limit, max/min Mass interaction? tuning this is weird/tricky   
        //TODO: there's probably bugs/some logic error in the uniform scale limiter -- look at it later
        if (targetInteractable.tag.Contains("XScalable"))
        {
            limitedScale = Math.Max(Math.Min(curScale.x * scaleRate, maxScale), minScale);
            if (targetInteractable.tag.Contains("Anchored"))
            {
                targetInteractable.GetComponentInParent<Transform>().parent.gameObject.transform.localScale = new Vector3(limitedScale, curScale.y, curScale.z);
            } else
            {
                targetInteractable.transform.localScale = new Vector3(limitedScale, curScale.y, curScale.z);
            }      
        }
        else if (targetInteractable.tag.Contains("YScalable"))
        {
            limitedScale = Math.Max(Math.Min(curScale.y * scaleRate, maxScale), minScale);
            if (targetInteractable.tag.Contains("Anchored"))
            {
                targetInteractable.GetComponentInParent<Transform>().parent.gameObject.transform.localScale = new Vector3(curScale.x, limitedScale, curScale.z);
            } else
            {
                targetInteractable.transform.localScale = new Vector3(curScale.x, limitedScale, curScale.z);
            }
        }
        else if (targetInteractable.tag.Contains("ZScalable"))
        {
            limitedScale = Math.Max(Math.Min(curScale.z * scaleRate, maxScale), minScale);
            if (targetInteractable.tag.Contains("Anchored"))
            {
                targetInteractable.GetComponentInParent<Transform>().parent.gameObject.transform.localScale = new Vector3(curScale.x, curScale.y, limitedScale);
            } else
            {
                targetInteractable.transform.localScale = new Vector3(curScale.x, curScale.y, limitedScale);
            }
        } else
        {
            limitedScale = Math.Max(Math.Min(Math.Max(Math.Max(curScale.x,curScale.y),curScale.z) * scaleRate, maxScale), minScale);
            targetInteractable.transform.localScale = new Vector3(curScale.x * scaleRate, curScale.y * scaleRate, curScale.z * scaleRate);
        }
    }

    /// <summary>
    /// Update object mass proportionally to it's scale (?and potentially material type?)
    /// </summary>
    /// <param name="targetInteractable"></param>
    /// <param name="massRate"></param>
    /// <param name="objectMaterial"></param>
    private void UpdateMass(GameObject targetInteractable, float massRate, string objectMaterial)
    {
        //TODO: Tuning mass on scaling, min/max Mass on a PER OBJECT basis
        float minMass = 0f; //Note: Unity mass units are kg
        float maxMass = 100f; 
        float currentTargetMass = targetInteractable.GetComponent<Rigidbody>().mass;
        if (targetInteractable.tag.Contains("XScalable") || targetInteractable.tag.Contains("YScalable") || targetInteractable.tag.Contains("ZScalable")) {
             //If scaling in only one dimension, scale by cubic root of the given massRate 
             massRate = (float)Math.Pow(massRate, (1.0f/3.0f)); //Pls no floating point errors
        }
        //Update mass given limiters [min|max]Mass
        targetInteractable.GetComponent<Rigidbody>().mass = Math.Max(minMass, Math.Min(currentTargetMass * massRate, maxMass));
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
