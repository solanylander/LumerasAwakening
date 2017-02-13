using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    //Raycast Variables
    [Range(5.0f, 40f)]
    public float maxRange = 25f;
    //public Transform lineOrigin;
    //public GameObject tracerEffect; - Particles

    private Vector3 rayOrigin;
    private LineRenderer tracerLine;
    private Camera playerCam;
    private float nextFire;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.05f);
    private RaycastHit hit; //holds info about anything hit by ray casted
    //private float hitForce = 250f; //for debugging

    //Targetting Variables
    public Material outlineMaterial; //TODO: Write up a nicer outline shader
    private Material defaultMaterial;
    private Renderer currentRenderer;
    private GameObject currentTarget;
    private int layerMask = 0;

    //Power-Related Variables
    [Range(0.01f, 0.1f)]
    public float powerScalar = 0.025f; //Scaling Rate
    [Range(10.0f, 100.0f)]
    public float maxScale = 50.0f; //Growth Limiter
    [Range(0.05f, 1f)]
    public float minScale = 0.1f; //Shrinkage Limiter

    private Vector3 spawnPosition;

    void Start()
    {
        //Note: script should be in a child to the player character's camera object
        playerCam = GetComponentInParent<Camera>();
        tracerLine = GetComponentInChildren<LineRenderer>();
        layerMask = (1 << LayerMask.NameToLayer("Interactable")); //Raycast bit mask by shifting index of 'Interactable' layer

        spawnPosition = transform.parent.gameObject.transform.parent.transform.position;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape) | transform.position.y < -100)
        {
            transform.parent.gameObject.transform.parent.transform.position = spawnPosition;
        }

        //Clear Target if out of range -- Removing when it leaves FoV doesn't feel fun/gud
        if (currentTarget != null && (currentTarget.transform.position - transform.position).magnitude > maxRange)
        {
            ClearTarget(currentTarget);
        }

        //Targetting & Scaling
        if ( (Input.GetButton("Fire1") | Input.GetButton("Fire2"))) 
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
            if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, maxRange, layerMask) && hit.rigidbody.gameObject.tag.Contains("Interactable"))
            {
                //tracerLine.SetPosition(1, hit.point); b/c Ugly
                if (hit.rigidbody != null)
                {
                    //For debugging purposes: hit obj @ normal to surface hit
                    //hit.rigidbody.AddForce(-hit.normal * hitForce);
                    
                    //Selecting/Lock on to new target
                    if (currentTarget != hit.rigidbody.gameObject)
                    {
                        SelectTarget(hit.rigidbody.gameObject);
                    } else if (Input.GetButton("Fire1"))
                    {
                        ScaleObject(hit.rigidbody.gameObject, 1 + powerScalar);
                    } else if (Input.GetButton("Fire2"))
                    {
                        ScaleObject(hit.rigidbody.gameObject, 1 - powerScalar);
                    }     
                }
            }
            else if (currentTarget != null)
            {
                //Just render the line of length maxRange
                //tracerLine.SetPosition(1, lineOrigin + (playerCam.transform.forward * maxRange)); b/c Ugly
                if (Input.GetButton("Fire1"))
                {
                    ScaleObject(currentTarget, 1 + powerScalar);
                }
                else if (Input.GetButton("Fire2"))
                {
                    ScaleObject(currentTarget, 1 - powerScalar);
                }
            }
        }
    }

    /// <summary>
    /// Highlights targetInteractable by changing Renderer material to outlineMaterial and sets curerntTarget object
    /// </summary>
    /// <param name="targetInteractable"></param>
    void SelectTarget(GameObject targetInteractable)
    {
        if (currentRenderer != null)
        {
            currentRenderer.material = defaultMaterial;
        }
        currentRenderer = targetInteractable.GetComponent<Renderer>();
        defaultMaterial = currentRenderer.material;
        currentRenderer.material = outlineMaterial;
        currentTarget = targetInteractable;
    }

    /// <summary>
    /// Clear the current target from lock on, remove visual indication & unset currentTarget
    /// </summary>
    /// <param name="targetInteractable"></param>
    void ClearTarget(GameObject targetInteractable)
    {
        currentRenderer = targetInteractable.GetComponent<Renderer>();
        currentRenderer.material = defaultMaterial;
        currentTarget = null;
    }

    /// <summary>
    /// Scale the target gameObject based on given scalar rate in the dimension specified by the object tag.
    /// Scaling is a muliplication operation to create some input acceleration (smaller: more granular, larger: quicker scaling)
    /// Anchored Interactables are children of invisible 'anchor' objects which redefine the pivot point for scaling.
    /// </summary>
    /// <param name="targetInteractable"></param>
    void ScaleObject(GameObject targetInteractable, float scaleRate)
    {
        float limitedScale;
        Vector3 curScale;
        if (targetInteractable.tag.Contains("Anchored"))
        {
            curScale = targetInteractable.GetComponentInParent<Transform>().parent.gameObject.transform.localScale;
        } else
        {
            curScale = targetInteractable.transform.localScale;
        }
            
        //TODO: there's probably bugs/some logic error in the uniform scale limiter -- look at it later
        if (targetInteractable.tag.Contains("XScalable"))
        {
            limitedScale = System.Math.Max(System.Math.Min(curScale.x * scaleRate, maxScale), minScale);
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
            limitedScale = System.Math.Max(System.Math.Min(curScale.y * scaleRate, maxScale), minScale);
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
            limitedScale = System.Math.Max(System.Math.Min(curScale.z * scaleRate, maxScale), minScale);
            if (targetInteractable.tag.Contains("Anchored"))
            {
                targetInteractable.GetComponentInParent<Transform>().parent.gameObject.transform.localScale = new Vector3(curScale.x, curScale.y, limitedScale);
            } else
            {
                targetInteractable.transform.localScale = new Vector3(curScale.x, curScale.y, limitedScale);
            }
        } else
        {
            limitedScale = System.Math.Max(System.Math.Min(System.Math.Max(System.Math.Max(curScale.x,curScale.y),curScale.z) * scaleRate, maxScale), minScale);
            targetInteractable.transform.localScale = new Vector3(curScale.x * scaleRate, curScale.y * scaleRate, curScale.z * scaleRate);
        }
    }

    /// <summary>
    /// Update object mass proportionally to it's scale (?and potentially material type?)
    /// </summary>
    /// <param name="targetInteractable"></param>
    /// <param name="objectMaterial"></param>
    void updateMass(GameObject targetInteractable, string objectMaterial)
    {
        //TODO: updating mass on scaling
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
