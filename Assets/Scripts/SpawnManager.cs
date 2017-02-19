using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    //Prototyping Stuff
    private Vector3 spawnPosition;
    private float deathDepth;
    private bool spawnSet;

    private float duration = 4500.0f;
    //private Color midGray = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Color darkGray = new Color(0.2f, 0.2f, 0.2f, 1f);

    void Start () {
        //Note: script should be in a child to the player character's camera object
        //Repsawn for prototyping
        spawnPosition = transform.parent.gameObject.transform.parent.transform.position;
        deathDepth = -100;
        spawnSet = false;
    }
	
	/// <summary>
    /// Respawn player at furthest progression checkpoint on death / reset
    /// </summary>
    /// <remarks>
    /// TODO: checkpoints, other game transitions, UI scene resets [probably move that functionality to SceneManager]
    /// </remarks>
	void Update () {
        //Repsawn for prototyping
        if (transform.position.y > 185 && !spawnSet)
        {
            spawnPosition = new Vector3(71, 188, -65);
            spawnSet = true;
            deathDepth = 100;
        }
        if (spawnSet)
        {
            //tmp testing level triggers
            Camera pCam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
            Color bgColor = pCam.backgroundColor;
            float t = Mathf.PingPong(Time.time, duration) / duration;
            pCam.backgroundColor = Color.Lerp(bgColor, darkGray, t);
        }
        if (Input.GetKeyDown(KeyCode.R) | transform.position.y < deathDepth)
        {
            transform.parent.gameObject.transform.parent.transform.position = spawnPosition;
        }
    }
}
