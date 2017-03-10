using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject target;
    public float attackRange = 10.0f;
    public float attackTickSpeed = 0.5f;
    public int attackDamage = 25;
    public float lookSpeed = 55.0f;

    private float attackTimer;
    private ResourceManager targetResources;

    private Vector3 lastKnownPosition;
    private Quaternion lookAtRotation;

    private LineRenderer beamLine;
    private int interactableMask;
    private int wallMask;

    private AudioSource audioSource;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        //targetResources = target.GetComponentInChildren<ResourceManager>();
        beamLine = GetComponent<LineRenderer>();
        beamLine.enabled = false;
        interactableMask = 1 << LayerMask.NameToLayer("Interactable");
        wallMask = 1 << LayerMask.NameToLayer("Wall");
        attackTimer = 0;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        beamLine.enabled = false;
        RaycastHit hit;
        Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.red, 1.0f);
        //Debug.DrawRay(transform.position, transform.forward, Color.blue, 1.0f);
        lastKnownPosition = target.transform.position;
        lookAtRotation = Quaternion.LookRotation(lastKnownPosition - transform.position, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtRotation, lookSpeed * Time.deltaTime); // Mathf.Pow(Mathf.Sin(Time.time * lookSpeed), 2)); //lookSpeed * Time.delta
        if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out hit, attackRange))
        {
            if (Quaternion.Angle(transform.rotation, lookAtRotation) < 5.0f && hit.collider.gameObject.tag.Equals("Player"))
            {
                //TODO: fix this, it's broken - range + walls
                beamLine.enabled = true;
                beamLine.SetPosition(0, transform.position);
                beamLine.SetPosition(1, target.transform.position);
                if (Time.time > attackTimer)
                {
                    //targetResources.damaged = true;
                    //targetResources.decrementResource(attackDamage);
                    attackTimer = Time.time + attackTickSpeed;
                    audioSource.Play();
                    //Insta-Gib if you get shot
                    GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().killPlayer();
                    //GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().decrementScore(25);
                }
            }
        }
    }
}


