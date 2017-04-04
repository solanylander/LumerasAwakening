using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour {

	public Transform teleporterEnd;
    private static float useDelay;
    private static float nextUse;

    void Start()
    {
        useDelay = 2.5f;
        nextUse = Time.time;
    }

    void OnTriggerEnter(Collider col)
    {
        if (Time.time >= nextUse && col.gameObject.tag.Contains("Player") && teleporterEnd != null)
        {
            nextUse = Time.time + useDelay;
            col.gameObject.transform.position = teleporterEnd.position;
            //play teleport sound
            GetComponent<AudioSource>().Play();
        }
    }
}
