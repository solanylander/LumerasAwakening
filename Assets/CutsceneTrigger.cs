using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour {
    public Camera playerCam;
    public Camera cutsceneCam;
    public GameObject cine;
    private GameObject player;
    public int time;
    bool played;
	// Use this for initialization
	void Start () {
        played = false;
        cine.SetActive(false);
        player = GameObject.Find("Test_Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collision) {
        if(!played) {
            played = true;
            cine.SetActive(true);
            //cutsceneCam.GetComponent<Animation>().Play("LaserEntrance");
            StartCoroutine(wait());
            
        }
    }

    IEnumerator wait() {
        playerCam.enabled = false;
        cutsceneCam.enabled = true;
        player.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(time);
        playerCam.enabled = true;
        cutsceneCam.enabled = false;
        player.GetComponent<CharacterController>().enabled = true;
        
    }
}
