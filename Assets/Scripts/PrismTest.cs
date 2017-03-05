using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismTest : MonoBehaviour {
    private RaycastHit hit;
    private LineRenderer beamLine;
    public Transform beamTarget;
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
    /// All garbage  rn
    /// Solution: use beam limit anchor (defines direction + limits)
    /// activate node beam if node is in contact with beam
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
                target.GetComponent<PrismTest>().ActivateBeam();
                if (gameObject.tag.Contains("Interactable"))
                {
                    beamLine.SetPosition(1, beamTarget.position);
                } else
                {
                    beamLine.SetPosition(1, beamTarget.position);
                }
                beamLine.enabled = true;
            }
        }
    }

    void ActivateBeam()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1), beamHeading, out hit, beamRange))
        {
            if (hit.collider.gameObject.tag.Contains("BeamNode"))
            {
                beamActive = true;
                target = hit.collider.gameObject;
                //target.GetComponent<PrismTest>().ActivateBeam();
                beamLine.SetPosition(1, beamTarget.transform.position);
                beamLine.enabled = true;
            }
        }
    }
}
