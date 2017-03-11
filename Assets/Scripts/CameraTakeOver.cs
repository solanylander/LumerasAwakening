using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTakeOver : MonoBehaviour {

    public Camera cineCam;
    public BeamEventTrigger triggerEvent;
    private GameObject playerObj;
    [SerializeField]
    private Camera playerCam;
    public bool activate;
    public static bool mutex;
    //todo play some cinematic sound during this

	void Start () {
        playerObj = GameObject.FindGameObjectWithTag("MainCamera");
        playerCam = playerObj.GetComponent<Camera>();
        activate = false;
        mutex = false;
    }
	
    /// <summary>
    /// Take over main camera to show event trigger from beams
    /// </summary>
	void Update () {
        if (activate)
        {
            cineCam.enabled = true;
        }
        else if (!activate)
        {
            cineCam.enabled = false;
        }
    }
}
