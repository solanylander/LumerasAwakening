using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    //Prototyping Stuff
    private Vector3 spawnPosition;
    private float deathDepth;
    private int spawnSet;

    private float duration = 9000.0f;
    //private Color midGray = new Color(0.5f, 0.5f, 0.5f, 1f); //orig HSV 0,0 175, 5
    private Color darkGray = new Color(0.2f, 0.2f, 0.2f, 1f);

    void Start () {
        //Note: script should be in a child to the player character's camera object
        //Repsawn for prototyping
        spawnPosition = transform.parent.gameObject.transform.parent.transform.position;
        deathDepth = -100;
        spawnSet = 0;
    }
	
	/// <summary>
    /// Respawn player at furthest progression checkpoint on death / reset
    /// </summary>
    /// <remarks>
    /// TODO: checkpoints, other game transitions, UI scene resets [probably move that functionality to SceneManager]
    /// </remarks>
	void Update () {
        //Repsawn for prototyping
        if (transform.position.y > 185 && spawnSet == 0)
        {
            spawnPosition = new Vector3(71, 188, -65);
            spawnSet = 1;
            deathDepth = 100;
        }

        if (spawnSet == 1)
        {
            //tmp testing level triggers
            Camera pCam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
            Color bgColor = pCam.backgroundColor;
            float t = Mathf.PingPong(Time.time, duration) / duration;
            pCam.backgroundColor = Color.Lerp(bgColor, darkGray, t);
        }

        if (transform.position.y > 315)
        {
            //set spawn position, etc.
            Camera pCam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
            Color bgColor = pCam.backgroundColor;
            float t = Mathf.PingPong(Time.time, duration) / duration;
            pCam.backgroundColor = Color.Lerp(bgColor, Color.black, t);
        }

        if (Input.GetKeyDown(KeyCode.R) | transform.position.y < deathDepth)
        {
            transform.parent.gameObject.transform.parent.transform.position = spawnPosition;
        }
    }
}
