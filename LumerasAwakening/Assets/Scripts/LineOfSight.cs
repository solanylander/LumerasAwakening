using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {

    public GameObject player;

    // Use this for initialization
    void Start () {
		
	}
    int check = 0;
    bool sight = false;
    //FixedUpdate
    void FixedUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        if (check > 4)
        {
            check = 0;

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
            Vector3 position = player.transform.position;
            go.transform.localScale = scale;
            go.AddComponent<LOSprojectile>();
            go.transform.GetComponent<LOSprojectile>().setDirection(direction);
            go.transform.GetComponent<LOSprojectile>().setPlayerPosition(position);
            go.tag = "Projectile";
            go.GetComponent<MeshRenderer>().enabled = false;
            go.transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z + direction.z);
            go.AddComponent<Rigidbody>(); // Add the rigidbody.
            go.GetComponent<LOSprojectile>().line = this;
        }else if(check == 2 && sight)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
            Vector3 position = player.transform.position;
            go.transform.localScale = scale;
            go.AddComponent<LOSprojectile>();
            go.transform.GetComponent<LOSprojectile>().setDirection(direction);
            go.transform.GetComponent<LOSprojectile>().setPlayerPosition(position);
            go.tag = "Projectile";
            go.GetComponent<MeshRenderer>().enabled = true;
            go.transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z + direction.z);
            go.AddComponent<Rigidbody>(); // Add the rigidbody.
            go.GetComponent<LOSprojectile>().line = this;
            check++;
        }
        else
        {
            check++;
        }
    }

    public void setSight(bool new_sight)
    {
        sight = new_sight;
    }
}
