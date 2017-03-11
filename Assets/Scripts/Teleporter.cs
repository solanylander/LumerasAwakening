using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

	public Transform teleporterEnd;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Contains("Player") && teleporterEnd != null)
        {
            col.gameObject.transform.position = teleporterEnd.position;
            //play teleport sound
        }
    }
}
