using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectBeam : MonoBehaviour {

    [SerializeField]
    private List<GameObject> nodesTargetingMe = new List<GameObject>();
    [SerializeField]
    private int numNodesTargetingMe;
    private Vector3 rayOrigin;
    private Vector3 beamHeading;
    private float beamRange;
    private RaycastHit hit;
    private Vector3 reflection;
    private LineRenderer reflectedBeam; //need to dynamically add these for multiple reflection
    // uhhhhh wait you can only have one linerenderer per object
    //keep a list of linerenderers? or instantiate a bunch of empties?
    //vectrosity on asset store for rendering lines
    //or go with gl method.... just to draw linessssss

	void Start () {
        reflectedBeam = GetComponent<LineRenderer>();
        numNodesTargetingMe = 0;
    }
	
	// Update is called once per frame
	void Update () {
        beamHeading = transform.position - nodesTargetingMe[0].transform.position;
        beamRange = Vector3.Distance(transform.position, nodesTargetingMe[0].transform.position);
        rayOrigin = new Vector3(nodesTargetingMe[0].GetComponent<Renderer>().bounds.center.x, nodesTargetingMe[0].GetComponent<Renderer>().bounds.center.y, nodesTargetingMe[0].GetComponent<Renderer>().bounds.center.z);
        Physics.Raycast(rayOrigin, beamHeading, out hit, beamRange * 5f);
        Debug.DrawRay(rayOrigin, beamHeading * 5f, Color.blue, 1.0f);

        Debug.DrawRay(transform.position, hit.normal * 5f, Color.green, 1.0f);

        reflectedBeam.enabled = false;
        reflection = Vector3.Reflect(beamHeading, hit.normal);
        Debug.DrawRay(transform.position, reflection * 5f, Color.yellow, 1.0f);
        reflectedBeam.SetPosition(0, transform.position);
        reflectedBeam.SetPosition(1, reflection * 5f);
    }

    public void addNodeTargetingMe(GameObject node)
    {
        //Other stuff
        if (!nodesTargetingMe.Contains(node))
        {
            nodesTargetingMe.Add(node);
            numNodesTargetingMe += 1;
        }
    }

    public void removeNodeTargetingMe(GameObject node)
    {
        //Other stuff
        if (nodesTargetingMe.Contains(node))
        {
            nodesTargetingMe.Remove(node);
            numNodesTargetingMe -= 1;
        }
    }
}
