using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private SpawnManager spawnManager;

	void Start () {
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
	}
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Contains("Player"))
        {
            spawnManager.spawnPosition = transform.position;
        }

    }
}
