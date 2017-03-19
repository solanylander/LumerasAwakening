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
    private GameObject target;

	void Start () {
        reflectedBeam = GetComponent<LineRenderer>();
        numNodesTargetingMe = 0;
        reflectedBeam.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        //only handles 1 beam being reflected atm, need to figure out how to work around not being able to have more than 1 line renderer
        if (nodesTargetingMe.Count > 0)
        {
            beamHeading = transform.position - nodesTargetingMe[0].transform.position;
            beamRange = Vector3.Distance(transform.position, nodesTargetingMe[0].transform.position);
            rayOrigin = new Vector3(nodesTargetingMe[0].GetComponent<Renderer>().bounds.center.x, nodesTargetingMe[0].GetComponent<Renderer>().bounds.center.y, nodesTargetingMe[0].GetComponent<Renderer>().bounds.center.z);
            Physics.Raycast(rayOrigin, beamHeading, out hit, beamRange * 5f);
            Debug.DrawRay(rayOrigin, beamHeading * 5f, Color.blue, 1.0f);

            Debug.DrawRay(transform.position, hit.normal * 5f, Color.green, 1.0f);

            reflection = Vector3.Reflect(beamHeading, hit.normal);
            //reflectedBeam.SetPosition(0, transform.position);
            //reflectedBeam.SetPosition(1, reflection);
            Debug.DrawRay(hit.point, reflection * 5f, Color.yellow, 1.0f);
            if (Physics.Raycast(hit.point, reflection, out hit, beamRange * 5f)) //will activate any objects it hits atm, bug
            {
                if (hit.collider.gameObject.tag.Contains("BeamNode"))
                {
                    reflectedBeam.enabled = true;
                    reflectedBeam.SetPosition(0, transform.position); // hit.point
                    target = hit.collider.gameObject;
                    //cheat don't use hit.point
                    reflectedBeam.SetPosition(1, hit.collider.gameObject.transform.position);
                    target.GetComponent<PrismTest>().ActivateBeam();
                    target.GetComponent<PrismTest>().addNodeTargetingMe(gameObject);
                } else if (hit.collider.gameObject.tag.Contains("Player"))
                {
                    GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().killPlayer();
                } else if (hit.collider.gameObject.tag.Contains("BeamReflector"))
                {
                    reflectedBeam.enabled = true;
                    reflectedBeam.SetPosition(0, transform.position); // hit.point
                    target = hit.collider.gameObject;
                    target.GetComponent<ReflectBeam>().addNodeTargetingMe(gameObject);
                    reflectedBeam.SetPosition(1, hit.collider.gameObject.transform.position);
                }
                else
                {
                    reflectedBeam.enabled = false;
                    if (target != null && target.GetComponent<PrismTest>() != null)
                    {
                        target.GetComponent<PrismTest>().DeactivateBeam();
                        target.GetComponent<PrismTest>().removeNodeTargetingMe(gameObject);
                    }
                    else if (target != null && target.GetComponent<ReflectBeam>() != null)
                    {
                        target.GetComponent<ReflectBeam>().removeNodeTargetingMe(gameObject);
                    }
                }
            }
            else 
            {
                //BUG: On source nodes reflecting into it and out will deactivate, breaking puzzles, need to tag source separately or something
                reflectedBeam.enabled = false;
                if (target != null && target.GetComponent<PrismTest>() != null)
                {
                    target.GetComponent<PrismTest>().DeactivateBeam();
                    target.GetComponent<PrismTest>().removeNodeTargetingMe(gameObject);
                }
                else if (target != null && target.GetComponent<ReflectBeam>() != null)
                {
                    target.GetComponent<ReflectBeam>().removeNodeTargetingMe(gameObject);
                }
            }
        }
        else //lmao this is so bad, fix later
        {
            //BUG: On source nodes reflecting into it and out will deactivate, breaking puzzles, need to tag source separately or something
            reflectedBeam.enabled = false;
            if (target != null && target.GetComponent<PrismTest>() != null)
            {
                target.GetComponent<PrismTest>().DeactivateBeam();
                target.GetComponent<PrismTest>().removeNodeTargetingMe(gameObject);
            }
            else if (target != null && target.GetComponent<ReflectBeam>() != null)
            {
                target.GetComponent<ReflectBeam>().removeNodeTargetingMe(gameObject);
            }
        }
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
