using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismTest : MonoBehaviour
{
    private RaycastHit hit;
    private LineRenderer beamLine;
    [SerializeField]
    private Transform beamTarget;
    public bool beamActive = false;
    private bool beamDefaultState;
    private float beamRange;
    private Vector3 beamHeading;
    private Vector3 basisY = Vector3.up;
    private Vector3 basisZ = Vector3.right;
    private Vector3 orthoNormalHeading;
    [SerializeField]
    private GameObject target;

    //Variant -- focusing / mutli-beam targetting for activation
    //[SerializeField]
    public List<GameObject> nodesTargettingMe = new List<GameObject>();
    //Check if list contains object that is trying to trigger activation, 
    //if not add - when removing remve node that is no longer targetting 
    //from the array list if it is in the list, if the list length is at 
    //least activation threshold, trigger the beam for this node 
    [SerializeField]
    private int numNodesTargetingMe;
    public int activationThreshold = 1;

    public enum BeamBoundOrigin
    {
        Default, XMax, XMin, ZMax, ZMin
    }
    public BeamBoundOrigin beamBoundOrigin;
    private Vector3 rayOrigin;
    [SerializeField]
    private float rayOffset;

    [SerializeField]
    private bool debugRay;

    private AudioSource beamAudio;

    void Start()
    {
        beamLine = GetComponent<LineRenderer>();
        beamRange = Vector3.Distance(transform.position, beamTarget.position);
        beamHeading = (beamTarget.position - transform.position);
        //Vector3.OrthoNormalize(ref beamHeading, ref basisY, ref basisZ);
        beamDefaultState = beamActive;
        activationThreshold = beamActive ? 0 : activationThreshold;
        numNodesTargetingMe = nodesTargettingMe.Count;
        //assuming always square
        rayOffset = gameObject.GetComponent<Renderer>().bounds.max.z - gameObject.GetComponent<Renderer>().bounds.min.z;
        beamAudio = GetComponent<AudioSource>();
        beamAudio.mute = true;
    }

    /// <summary>
    /// Solution: use beam 'target' anchor (defines direction heading + range limits)
    /// Activate node beam if node is in contact with beam, deactivate if not
    /// This is is completely modular, you can have as many nodes of any object in series as you want in any configuration
    /// </summary>
    /// <remarks>
    /// Need to optimize this, can't really mask the raycasts with a bitmask since they interact with pretty much everything, range is as close to low as possible
    /// Frequency of updates needs to be evaluated, since they are in an update, it's pretty overkill given the number of nodes we may be dealing with
    /// </remarks>
    void Update()
    {
        switch(beamBoundOrigin)
        {
            //TODO implement this, ray origin based on renderer bounds 
            case BeamBoundOrigin.Default:
                rayOrigin = new Vector3(gameObject.GetComponent<Renderer>().bounds.center.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.center.z);
                break;
            case BeamBoundOrigin.XMax:
                rayOrigin = new Vector3(gameObject.GetComponent<Renderer>().bounds.max.x, gameObject.GetComponent<Renderer>().bounds.max.y, gameObject.GetComponent<Renderer>().bounds.center.z);
                break;
            case BeamBoundOrigin.XMin:
                rayOrigin = new Vector3(gameObject.GetComponent<Renderer>().bounds.min.x, gameObject.GetComponent<Renderer>().bounds.max.y, gameObject.GetComponent<Renderer>().bounds.center.z);
                break;
            case BeamBoundOrigin.ZMax:
                new Vector3(gameObject.GetComponent<Renderer>().bounds.center.x, gameObject.GetComponent<Renderer>().bounds.max.y, gameObject.GetComponent<Renderer>().bounds.max.z);
                break;
            case BeamBoundOrigin.ZMin:
                new Vector3(gameObject.GetComponent<Renderer>().bounds.center.x, gameObject.GetComponent<Renderer>().bounds.max.y, gameObject.GetComponent<Renderer>().bounds.min.z);
                break;
        }
        // maxX rayOrigin = new Vector3(gameObject.GetComponent<Renderer>().bounds.max.x, gameObject.GetComponent<Renderer>().bounds.max.y, gameObject.GetComponent<Renderer>().bounds.center.z);
        // minX rayOrigin = new Vector3(gameObject.GetComponent<Renderer>().bounds.min.x, gameObject.GetComponent<Renderer>().bounds.max.y, gameObject.GetComponent<Renderer>().bounds.center.z);
        // maxZ rayOrigin = new Vector3(gameObject.GetComponent<Renderer>().bounds.center.x, gameObject.GetComponent<Renderer>().bounds.max.y, gameObject.GetComponent<Renderer>().bounds.max.z);
        // minZ rayOrigin = new Vector3(gameObject.GetComponent<Renderer>().bounds.center.x, gameObject.GetComponent<Renderer>().bounds.max.y, gameObject.GetComponent<Renderer>().bounds.min.z);
        if (beamLine.enabled)
        {
            beamAudio.mute = false;
        }
        else
        {
            beamAudio.mute = true;
        }

        if (gameObject.tag.Contains("Interactable"))
        {
            beamLine.SetPosition(0, rayOrigin);
        }
        else
        {
            beamLine.SetPosition(0, rayOrigin);
        }

        Debug.DrawRay(rayOrigin, beamHeading * 5f, Color.red, 1.0f);
        //Debug.Log(beamHeading);
        debugRay = Physics.Raycast(rayOrigin, beamHeading, out hit, beamRange * 5f) && hit.collider.gameObject.tag.Contains("BeamNode");

        if (Physics.Raycast(rayOrigin, beamHeading, out hit, beamRange * 5f) && beamActive.Equals(true))
        {
            if (hit.collider.gameObject.tag.Contains("BeamNode"))
            {
                target = hit.collider.gameObject;
                //Activations chain to next node in series
                target.GetComponent<PrismTest>().ActivateBeam();
                target.GetComponent<PrismTest>().addNodeTargetingMe(gameObject);
                if (gameObject.tag.Contains("Interactable"))
                {
                    beamLine.SetPosition(1, target.transform.position);
                }
                else
                {
                    beamLine.SetPosition(1, beamTarget.position);
                }
                beamLine.enabled = true;
            }
            else if (hit.collider.gameObject.tag.Contains("Player") && beamLine.enabled)
            {
                GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().killPlayer();
            } else if (hit.collider.gameObject.tag.Contains("BeamReflector"))
            {
                beamActive = true;
                beamLine.SetPosition(1, beamTarget.position);
                beamLine.enabled = true;
                target = hit.collider.gameObject;
                target.GetComponent<ReflectBeam>().addNodeTargetingMe(gameObject);
                beamLine.SetPosition(1, target.transform.position);
            } else
            {
                beamLine.enabled = false;
                if (target != null && target.GetComponent<PrismTest>() != null)
                {
                    //Deactivations chain to next node in series
                    target.GetComponent<PrismTest>().removeNodeTargetingMe(gameObject);
                    if (target.GetComponent<PrismTest>().numNodesTargetingMe < target.GetComponent<PrismTest>().activationThreshold)
                    {
                        target.GetComponent<PrismTest>().DeactivateBeam();
                    }
                } else if (target != null && target.GetComponent<ReflectBeam>() != null)
                {
                    target.GetComponent<ReflectBeam>().removeNodeTargetingMe(gameObject);
                }
            }
        }
        else
        {
            beamLine.enabled = false;
            if (target != null && target.GetComponent<PrismTest>() != null)
            {
                //Deactivations chain to next node in series
                target.GetComponent<PrismTest>().DeactivateBeam();
                target.GetComponent<PrismTest>().removeNodeTargetingMe(gameObject);
                target = null;
            } else if (target != null && target.GetComponent<ReflectBeam>() != null)
            {
                target.GetComponent<ReflectBeam>().removeNodeTargetingMe(gameObject);
            }
        }
    }

    /// <summary>
    /// Activate beam nodes - triggers in sequence for chained nodes
    /// </summary>
    public void ActivateBeam()
    {
        if (Physics.Raycast(rayOrigin, beamHeading * 5f, out hit, beamRange))
        {
            if (hit.collider.gameObject.tag.Contains("BeamNode") && numNodesTargetingMe >= activationThreshold)
            {
                beamActive = true;
                target = hit.collider.gameObject;
                //target.GetComponent<PrismTest>().ActivateBeam();
                if (gameObject.tag.Contains("Interactable"))
                {
                    beamLine.SetPosition(1, target.transform.position);
                }
                else
                {
                    beamLine.SetPosition(1, beamTarget.position);
                }
                beamLine.enabled = true;
            }
            else if (hit.collider.gameObject.tag.Contains("Player") && beamLine.enabled)
            {
                GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().killPlayer();
            } else if (hit.collider.gameObject.tag.Contains("BeamReflector"))
            {
                beamActive = true;
                beamLine.SetPosition(1, beamTarget.position);
                beamLine.enabled = true;
                target = hit.collider.gameObject;
                target.GetComponent<ReflectBeam>().addNodeTargetingMe(gameObject);
                beamLine.SetPosition(1, target.transform.position);
            } else if (numNodesTargetingMe < activationThreshold) {
                beamActive = false;
                beamLine.enabled = false;
            }
        }
    }

    /// <summary>
    /// Dectivate beam nodes - triggers in sequence for chained nodes 
    /// </summary>
    public void DeactivateBeam()
    {
        beamActive = false;
        beamLine.enabled = false;
    }

    public void addNodeTargetingMe(GameObject node)
    {
        //Other stuff
        if (!nodesTargettingMe.Contains(node))
        {
            nodesTargettingMe.Add(node);
            numNodesTargetingMe += 1;
        }
    }

    public void removeNodeTargetingMe(GameObject node)
    {
        //Other stuff
        if (nodesTargettingMe.Contains(node))
        {
            nodesTargettingMe.Remove(node);
            numNodesTargetingMe -= 1;
        }
    }
}