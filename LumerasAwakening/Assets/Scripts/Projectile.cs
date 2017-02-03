using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Vector3 direction;
    public Vector3 playerLocation;
    public LineOfSight line;
    public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 0.001f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        rb.AddForce(direction / 8.0f);
    }

    public void setDirection(Vector3 new_direction)
    {
        direction = new_direction;
    }

    public void setPlayerPosition(Vector3 new_position)
    {
        playerLocation = new_position - direction;
    }


    void OnCollisionEnter(Collision collision)
    {
        if ("Play" == collision.gameObject.tag)
        {
            Destroy(gameObject);
        }
        else if ("LOSprojectile" != collision.gameObject.tag)
        {
            Destroy(gameObject);
        }
    }
}