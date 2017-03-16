using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetingController))]
[RequireComponent(typeof(AudioSource))]
public class PowerController : MonoBehaviour
{
    //Switches
    [RangeAttribute(0,1)]
    public int targetLockOn = 1;
    [RangeAttribute(0, 1)]
    public int softLockOn = 0;
    [RangeAttribute(0, 1)]
    public int lazerBeams = 0;
    [RangeAttribute(0, 1)]
    public int freezeWhileScaling = 1;
    [RangeAttribute(0, 1)]
    public int globalScaleDecay = 1;
    [RangeAttribute(0, 1)]
    public int resources = 0;

    private Vector3 rayOrigin;
    private LineRenderer tracerLine;
    private Camera playerCam;
    private RaycastHit hit;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.05f);
    //private float hitForce = 250f; //for debugging
    //public Transform lineOrigin;
    //public GameObject tracerEffect; - Particles
    private TargetingController targetingController;
    private int layerMask = 0;
    [SerializeField]
    private bool getNewTarget;
    private bool enableScaleDecay;

    //Scaling Variables
    [Range(5.0f, 10000f)]
    public float maxPowerRange = 40f;
    [Range(0.01f, 0.1f)]
    public float powerScalar = 0.015f; //Scaling Rate
    [Range(0.0f, 1.0f)]
    public float massScalar = 0.025f; //Mass Scaling Rate
    [Range(0.0f, 1f)]
    public float scaleDelay = 0.35f;
    [Range(0.0f,1.0f)]
    public float energyDrainPerTick = 1.0f;
    private float scaleStart;
    private ResourceManager resourceManager;

    public AudioClip scaleUpAudio;
    public AudioClip scaleDownAudio;
    private AudioSource audioSource;
    public List<AudioClip> outOfRangeAudio;
    public AudioClip teleporterChirp;
    private AudioSource chirpSource;
    public GameObject spellEffect;

    private GameObject lastMouseover;
    private float startPowerVolume;

    void Start()
    {
        //Note: script should be in a child to the player character's camera object
        //playerCam = GetComponentInParent<Camera>();
        playerCam = GetComponentInParent<Camera>();
        tracerLine = GetComponentInChildren<LineRenderer>();
        layerMask = (1 << LayerMask.NameToLayer("Interactable")); //Raycast bit mask by shifting index of 'Interactable' layer
        layerMask = layerMask | (1 << LayerMask.NameToLayer("Wall"));
        layerMask = layerMask | (1 << LayerMask.NameToLayer("BeamNodes"));
        targetingController = transform.gameObject.GetComponent<TargetingController>();
        getNewTarget = true;
        //uhhh probably a better way to do this ... :o
        enableScaleDecay = globalScaleDecay == 1 ? true : false;
        audioSource = GetComponent<AudioSource>();
        chirpSource = GetComponents<AudioSource>()[1];
        resourceManager = GetComponent<ResourceManager>();
        //Todo: more gracefully stop/start spell effect
        spellEffect = GameObject.FindGameObjectWithTag("SpellEffect");
        startPowerVolume = audioSource.volume;
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
                    if (audioSource.isPlaying)
                    {
                        //audioSource.Stop();
                        StartCoroutine(AudioFadeOut.FadeOut (audioSource, 0.15f));
                        audioSource.volume = startPowerVolume;
                    }
                    spellEffect.SetActive(false);
                }
                break;
        }

        Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));


        //THIS IS BUGGED, REMOVING FOR THE UBISOFT PLAYTEST
        //In range color on mouse over -- offset range by -3 units because *Reasons*
        //if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, maxPowerRange - 3.0f, layerMask) && hit.rigidbody != null && hit.rigidbody.gameObject.tag.Contains("Interactable"))
        //{       
        //    //In Range, change outline color  
        //    if (hit.collider.gameObject.GetComponent<Interactable>() != null) {
        //        hit.collider.gameObject.GetComponent<Interactable>().setOutline = true;
        //        lastMouseover = hit.collider.gameObject;
        //    }
        //} else {
        //    if (lastMouseover != null && targetingController.currentTarget == null) {
        //        lastMouseover.GetComponent<Interactable>().setOutline = false;
        //    }
        //}
        
        //Scaling object when 'shot'
        if ( (Input.GetButton("Fire1") | Input.GetButton("Fire2")) && Time.time > scaleStart )  //TODO: Gamepad mappings 
        {
            spellEffect.SetActive(true);
            //Renders a line, add in spell effect when/if ready, remove for now b/c Ugly
            //StartCoroutine(ShotEffect());

            /* translates where the point is in the viewport to the world coordinate system
            top left = 0, top right = 1, bot left = 1
            translate middle of viewport in x and y to point in world coordinates */
            //Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            //Set LineRenderer position to public origin transform
            //tracerLine.SetPosition(0, this.lineOrigin.position); b/c Ugly

            //Out of range check
            if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, Mathf.Infinity, layerMask) && hit.rigidbody != null && hit.rigidbody.gameObject.tag.Contains("Interactable"))
            {
                if (Vector3.Distance(transform.position, hit.collider.gameObject.transform.position)  > maxPowerRange)
                {
                    if (!chirpSource.isPlaying)
                    {
                        chirpSource.clip = outOfRangeAudio[UnityEngine.Random.Range(0, outOfRangeAudio.Count)];
                        chirpSource.Play();
                    }
                }
            } 


            //Ray registers a hit with an Interactable object
            //removed layermask from raycast to prevent scaling through walls since they are no longer transparent
            if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, maxPowerRange, layerMask) && hit.rigidbody != null && hit.rigidbody.gameObject.tag.Contains("Interactable"))
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
                        if (targetingController.currentTarget.tag.Contains("Slow"))
                        {
                            ScaleObject(targetingController.currentTarget, 1.03f);
                        }
                        else
                        {
                            ScaleObject(targetingController.currentTarget, 1 + powerScalar);
                        }
                        if (!audioSource.isPlaying)
                        {
                            audioSource.clip = scaleUpAudio;
                            audioSource.Play();
                        }
                    } else if (Input.GetButton("Fire2") && targetingController.currentTarget != null)
                    {
                        ScaleObject(targetingController.currentTarget, 1 - powerScalar);
                        if (!audioSource.isPlaying)
                        {
                            audioSource.clip = scaleDownAudio;
                            audioSource.Play();
                        }
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
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = scaleUpAudio;
                        audioSource.Play();
                    }
                } else if (Input.GetButton("Fire2"))
                {  
                    ScaleObject(targetingController.currentTarget, 1 - powerScalar);
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = scaleDownAudio;
                        audioSource.Play();
                    }
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
    /// TODO: Implement a log f'n for scaling multiplier based on current X|Y|Z scale val.
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
        Interactable anchorInteractionController = targetInteractable.GetComponent<Interactable>(); //Complains if unassigned even if use is unreachable code if unassigned
        float limitedScale;
        Vector3 curScale;
        //Anchored objects are the children of anchor objects which are used to 'change' the pivot point of the object
        if (targetInteractable.tag.Contains("Anchored"))
        {
            anchorInteractionController = targetInteractable.GetComponentInParent<Transform>().parent.gameObject.GetComponent<Interactable>();
            curScale = targetInteractable.GetComponentInParent<Transform>().parent.gameObject.transform.localScale;
        } else
        {
            curScale = targetInteractable.transform.localScale;
        }
         
        if (targetInteractable.tag.Contains("XScalable"))
        {
            limitedScale = Math.Max(Math.Min(curScale.x * scaleRate, interactionController.maxScale), interactionController.minScale);
            if (targetInteractable.tag.Contains("Anchored"))
            {
                anchorInteractionController.updateScale(new Vector3(limitedScale, curScale.y, curScale.z), enableScaleDecay);
            } else
            {
                interactionController.updateScale(new Vector3(limitedScale, curScale.y, curScale.z), enableScaleDecay);
            }
            curScale = targetInteractable.transform.localScale;
        }

        if (targetInteractable.tag.Contains("YScalable"))
        {
            limitedScale = Math.Max(Math.Min(curScale.y * scaleRate, interactionController.maxScale), interactionController.minScale);
            if (targetInteractable.tag.Contains("Anchored"))
            {
               anchorInteractionController.updateScale(new Vector3(curScale.x, limitedScale, curScale.z),enableScaleDecay);
            } else
            {
                interactionController.updateScale(new Vector3(curScale.x, limitedScale, curScale.z), enableScaleDecay);
            }
            curScale = targetInteractable.transform.localScale;
        }

        if (targetInteractable.tag.Contains("ZScalable"))
        {
            limitedScale = Math.Max(Math.Min(curScale.z * scaleRate, interactionController.maxScale), interactionController.minScale);
            if (targetInteractable.tag.Contains("Anchored"))
            {
                anchorInteractionController.updateScale(new Vector3(curScale.x, curScale.y, limitedScale), enableScaleDecay);
            } else
            {
                interactionController.updateScale(new Vector3(curScale.x, curScale.y, limitedScale), enableScaleDecay);
            }
            curScale = targetInteractable.transform.localScale;
        } 

        //Set constraints back to originals if RB Interactable, re-enable collisions, update object mass
        targetRB.detectCollisions = true;
        targetRB.constraints = originalConstraints;
        float massRate = 1.0f;
        massRate = (scaleRate > 1.0f) ? (massRate + massScalar) : ((scaleRate < 1.0f) ? (massRate - massScalar): massRate);
        UpdateMass(targetInteractable, massRate, "placeHolder");
    }

    /// <summary>
    /// Update object mass proportionally to it's scale and specified mass scaling rate
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

