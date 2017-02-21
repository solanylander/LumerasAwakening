using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject target;
    public float attackRange = 10.0f;
    private float lookSpeed = 55.0f;

    private Vector3 lastKnownPosition;
    private Quaternion lookAtRotation;

    private LineRenderer beamLine;

    private int interactableMask;
    private int interactableWallMask;

    void Start()
    {
        beamLine = GetComponent<LineRenderer>();
        beamLine.enabled = false;
        interactableMask = 1 << LayerMask.NameToLayer("Interactable");
        interactableWallMask = (1 << LayerMask.NameToLayer("Wall")) & interactableMask;
    }

    void FixedUpdate()
    {
        beamLine.enabled = false;
        RaycastHit hit;
        Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.red, 1.0f);
        Debug.DrawRay(transform.position, transform.forward, Color.blue, 1.0f);
        if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, attackRange))
        {
            lastKnownPosition = target.transform.position;
            lookAtRotation = Quaternion.LookRotation(lastKnownPosition - transform.position, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtRotation, lookSpeed * Time.deltaTime); // Mathf.Pow(Mathf.Sin(Time.time * lookSpeed), 2)); //lookSpeed * Time.delta
            if (Quaternion.Angle(transform.rotation, lookAtRotation) < 5.0f && !Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, attackRange, interactableMask)) //TODO: check walls / selected - update layer so we can mask it out here
            {
                beamLine.enabled = true;
                beamLine.SetPosition(0, transform.position);
                beamLine.SetPosition(1, target.transform.position);
                //TODO: update to interact with resource once implemented
            } else
            {
                beamLine.enabled = false;
            }
        }
    }
}


