using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismTest : MonoBehaviour {
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

	void Start () {
        beamLine = GetComponent<LineRenderer>();
        beamRange = Vector3.Distance(transform.position, beamTarget.position);
        beamHeading = (beamTarget.position - transform.position);
        //Vector3.OrthoNormalize(ref beamHeading, ref basisY, ref basisZ);
        beamDefaultState = beamActive;
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
            beamLine.SetPosition(0, new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1.0f));
        }
        else
        {
            beamLine.SetPosition(0, new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1.0f));
        }

        Debug.DrawRay(new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1), beamHeading * 50f, Color.red, 1.0f);
        //Debug.Log(beamHeading);
        if (Physics.Raycast(new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1.0f), beamHeading, out hit, beamRange) && beamActive.Equals(true))
        {
            if (hit.collider.gameObject.tag.Contains("BeamNode"))
            {
                target = hit.collider.gameObject;
                //Activations chain to next node in series
                target.GetComponent<PrismTest>().ActivateBeam();
                if (gameObject.tag.Contains("Interactable"))
                {
                    beamLine.SetPosition(1, target.transform.position);
                } else
                {
                    beamLine.SetPosition(1, beamTarget.position);
                }
                beamLine.enabled = true;
            }
        } else
        {
            beamLine.enabled = false;
            if (target.GetComponent<PrismTest>() != null)
            {
                //Deactivations chain to next node in series
                target.GetComponent<PrismTest>().DeactivateBeam();
            }
        }
    }

    /// <summary>
    /// Activate beam nodes - triggers in sequence for chained nodes
    /// </summary>
    void ActivateBeam()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1), beamHeading, out hit, beamRange))
        {
            if (hit.collider.gameObject.tag.Contains("BeamNode"))
            {
                beamActive = true;
                target = hit.collider.gameObject;
                //target.GetComponent<PrismTest>().ActivateBeam();
                beamLine.SetPosition(1, target.transform.position);
                beamLine.enabled = true;
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
}
