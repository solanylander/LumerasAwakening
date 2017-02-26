using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour {
    //Prototyping Stuff
    [HideInInspector]
    public Vector3 spawnPosition;
    private float deathDepth;
    private int spawnSet;

    private float duration = 9000.0f;
    //private Color midGray = new Color(0.5f, 0.5f, 0.5f, 1f); //orig HSV 0,0 175, 5
    private Color darkGray = new Color(0.2f, 0.2f, 0.2f, 1f);
    private AudioSource audioSource;
    private bool[] audioPlayed = new bool[] { false, false, false };

    public GameObject playerCharacter;
    public AudioClip deathSound;
    public AudioClip otherSound;

    private ResourceManager resourceManager;

    void Start () {
        //Note: script should be in a child to the player character's camera object
        //Repsawn for prototyping
        spawnPosition = playerCharacter.transform.position;
        deathDepth = -100;
        spawnSet = 0;
        audioSource = GetComponent<AudioSource>();
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ResourceManager>();
        audioSource.clip = deathSound;
        audioSource.Play();
    }
	
	/// <summary>
    /// Respawn player at furthest progression checkpoint on death / reset
    /// </summary>
    /// <remarks>
    /// TODO: checkpoints, other game transitions, UI scene resets [probably move that functionality to SceneManager]
    /// </remarks>
	void Update () {
        //Repsawn for prototyping
        if (playerCharacter.transform.position.y > 185 && spawnSet == 0)
        {
            Debug.Log(audioPlayed[0]);
            if (!audioPlayed[0])
            {
                audioSource.clip = otherSound;
                audioSource.Play();
                audioPlayed[0] = true;
            }
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
            if (pCam.backgroundColor.Equals(darkGray))
            {
                spawnSet = 2;
            }
        }

        if (transform.position.y > 315 && spawnSet == 2)
        {
            if (!audioPlayed[1])
            {
                audioSource.clip = otherSound;
                audioSource.Play();
                audioPlayed[1] = true;
            }
            //set spawn position, etc.
            Camera pCam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
            Color bgColor = pCam.backgroundColor;
            float t = Mathf.PingPong(Time.time, duration) / duration;
            pCam.backgroundColor = Color.Lerp(bgColor, Color.black, t);
            if (pCam.backgroundColor.Equals(Color.black))
            {
                spawnSet = 3;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) | playerCharacter.transform.position.y < deathDepth)
        {
            if (spawnSet == 0)
            {
                SceneManager.LoadScene("la-3");
            } else
            {
                playerCharacter.transform.position = spawnPosition;
            }
            audioSource.clip = deathSound;
            audioSource.Play();
            resourceManager.currentResource = resourceManager.maxResource;
        }
    }
}
