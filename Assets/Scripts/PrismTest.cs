using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismTest : MonoBehaviour {
    private LineRenderer beamLine;
    private RaycastHit hit;
    private float beamRange;
    public GameObject target;

	void Start () {
        beamLine = GetComponent<LineRenderer>();
        if (target != null)
        {
            beamRange = Vector3.Distance(transform.position, target.transform.position);
        } else
        {
            beamRange = 10;
        }
	}
	
    /// <summary>
    /// All garbage  rn
    /// Solution: use beam limit anchor (defines direction + limits)
    /// activate node beam if node is in contact with beam
    /// </summary>
	void Update () {
        beamLine.enabled = true;
        if (gameObject.tag.Contains("Interactable"))
        {
            beamLine.SetPosition(0, new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1));
        } else
        {
            beamLine.SetPosition(0, transform.position);
        }

        if (target != null)
        {
            Debug.DrawRay(new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1), transform.forward, Color.blue, 1.0f);
            if (Physics.Raycast(new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1), transform.forward, out hit, beamRange))
            {
                if (hit.collider.gameObject.tag.Contains("beamReceiver"))
                {
                    target = hit.collider.gameObject;
                }
            }
            beamLine.SetPosition(1, target.transform.position);
        } else
        {
            Debug.DrawRay(new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1), transform.forward, Color.blue, 1.0f);
            if (Physics.Raycast(new Vector3(transform.position.x, gameObject.GetComponent<Renderer>().bounds.center.y, gameObject.GetComponent<Renderer>().bounds.max.z - 1), transform.forward, out hit, beamRange))
            {
                if (hit.collider.gameObject.tag.Contains("beamReceiver"))
                {
                    target = hit.collider.gameObject;
                    beamLine.SetPosition(1, target.transform.position);
                }
            } else
            {
                beamLine.SetPosition(1, transform.forward);
            }
           
        }
    }
}
