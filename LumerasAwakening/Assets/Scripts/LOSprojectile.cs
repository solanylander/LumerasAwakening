using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSprojectile : MonoBehaviour
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

        rb.AddForce(direction/8.0f);
        if (transform.position.x < playerLocation.x && direction.x < 0.0f || transform.position.x >= playerLocation.x && direction.x >= 0.0f)
        {
            if (transform.position.z + direction.z < playerLocation.z && direction.z < 0.0f || transform.position.z + direction.z >= playerLocation.z && direction.z >= 0.0f)
            {
                Destroy(gameObject);
                line.setSight(true);
            }
        }
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
            line.setSight(true);
        }
        else if("Projectile" != collision.gameObject.tag)
        {
            Destroy(gameObject);
            line.setSight(false);
        }
    }
}
