using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prettySpinningStuff : MonoBehaviour {
    public Transform axis;
    public bool RotateX = true;
    public bool RotateY = true;
    public bool RotateZ = true;

    private Vector3 pivot;

    void Start ()
    {
        pivot = new Vector3(axis.position.x, transform.position.y, axis.position.x);
    }
    void Update () {
        transform.position += (transform.rotation * pivot);

        if (RotateX)
        {
<<<<<<< HEAD
            transform.rotation *= Quaternion.AngleAxis(4 * Time.deltaTime, Vector3.right);
        }
        if (RotateY)
        {
            transform.rotation *= Quaternion.AngleAxis(4 * Time.deltaTime, Vector3.up);
        }
        if (RotateZ)
        {
            transform.rotation *= Quaternion.AngleAxis(4 * Time.deltaTime, Vector3.forward);
=======
            transform.rotation *= Quaternion.AngleAxis(3 * Time.deltaTime, Vector3.right);
        }
        if (RotateY)
        {
            transform.rotation *= Quaternion.AngleAxis(3 * Time.deltaTime, Vector3.up);
        }
        if (RotateZ)
        {
            transform.rotation *= Quaternion.AngleAxis(3 * Time.deltaTime, Vector3.forward);
>>>>>>> ff62596c7a5b6e2e44ff612af4f1e361ff69cb4f
        }

        transform.position -= (transform.rotation * pivot);
        //transform.RotateAround(pivot, Vector3.up, 0.1f * Time.deltaTime);
	}
}
