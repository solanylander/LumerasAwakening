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
    [SerializeField]
    private List<GameObject> nodesTargettingMe = new List<GameObject>();
    //Check if list contains object that is trying to trigger activation, 
    //if not add - when removing remve node that is no longer targetting 
    //from the array list if it is in the list, if the list length is at 
    //least activation threshold, trigger the beam for this node 
    [SerializeField]
    private int numNodesTargetingMe;
    public int activationThreshold = 1;

    void Start()
    {
        beamLine = GetComponent<LineRenderer>();
        beamRange = Vector3.Distance(transform.position, beamTarget.position);
        beamHeading = (beamTarget.position - transform.position);
        //Vector3.OrthoNormalize(ref beamHeading, ref basisY, ref basisZ);
        beamDefaultState = beamActive;
        activationThreshold = beamActive ? 0 : activationThreshold;
        numNodesTargetingMe = nodesTargettingMe.Count;
    }

    /// <summary>
    /// Solution: use beam 'target' anchor (defines direction heading + range limits)
    /// Activate node beam if node is in contact with beam, deactivate if not
    /// This is is completely modular, you can have as many nodes of any object in series as you want in any configuration
    /// </summary>
    void Update()
    {
        if (gameObject.tag.Contains("Interactable"))
        {
            beamLine.SetPosition(0, new Vector3(gameObject.GetComponent<Renderer>().bounds.center.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.center.z));
        }
        else
        {
            beamLine.SetPosition(0, new Vector3(gameObject.GetComponent<Renderer>().bounds.center.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.center.z));
        }

        Debug.DrawRay(new Vector3(gameObject.GetComponent<Renderer>().bounds.center.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.center.z), beamHeading * 50f, Color.red, 1.0f);
        //Debug.Log(beamHeading);
        if (Physics.Raycast(new Vector3(gameObject.GetComponent<Renderer>().bounds.center.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.center.z), beamHeading, out hit, beamRange * 5f) && beamActive.Equals(true))
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
            } else if (hit.collider.gameObject.tag.Contains("Player"))
            {
                GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().killPlayer();
            }
        }
        else
        {
            beamLine.enabled = false;
            if (target.GetComponent<PrismTest>() != null)
            {
                //Deactivations chain to next node in series
                target.GetComponent<PrismTest>().DeactivateBeam();
                target.GetComponent<PrismTest>().removeNodeTargetingMe(gameObject);
            }
        }
    }

    /// <summary>
    /// Activate beam nodes - triggers in sequence for chained nodes
    /// </summary>
    void ActivateBeam()
    {
        if (Physics.Raycast(new Vector3(gameObject.GetComponent<Renderer>().bounds.center.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.center.z), beamHeading * 5f, out hit, beamRange))
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
            else {
                beamActive = false;
                beamLine.enabled = false;
            }
        }
    }

    /// <summary>
    /// Dectivate beam nodes - triggers in sequence for chained nodes 
    /// </summary>
    void DeactivateBeam()
    {
        beamActive = false;
        beamLine.enabled = false;
    }

    void addNodeTargetingMe(GameObject node)
    {
        //Other stuff
        if (!nodesTargettingMe.Contains(node))
        {
            nodesTargettingMe.Add(node);
            numNodesTargetingMe += 1;
        }
    }

    void removeNodeTargetingMe(GameObject node)
    {
        //Other stuff
        if (nodesTargettingMe.Contains(node))
        {
            nodesTargettingMe.Remove(node);
            numNodesTargetingMe -= 1;
        }
    }
}