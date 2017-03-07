using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour {
    //Prototyping Stuff
    private Vector3 spawnPosition;
    private float deathDepth;
    private int spawnSet;
    bool isPause;
    Rect MainMenu = new Rect((Screen.width-200)/2, (Screen.height-200)/2, 200, 200);

    private float duration = 9000.0f;
    //private Color midGray = new Color(0.5f, 0.5f, 0.5f, 1f); //orig HSV 0,0 175, 5
    private Color darkGray = new Color(0.2f, 0.2f, 0.2f, 1f);
    private AudioSource deathAudio;
    public AudioClip deathSound;
    public AudioClip otherSound;

    void Start () {
        //Note: script should be in a child to the player character's camera object
        //Repsawn for prototyping
        spawnPosition = transform.parent.gameObject.transform.parent.transform.position;
        deathDepth = -100;
        spawnSet = 0;
        deathAudio = GetComponent<AudioSource>();
        isPause = false;
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
            deathAudio.clip = otherSound;
            deathAudio.Play();
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
            //set spawn position, etc.
            deathAudio.clip = otherSound;
            deathAudio.Play();
            Camera pCam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
            Color bgColor = pCam.backgroundColor;
            float t = Mathf.PingPong(Time.time, duration) / duration;
            pCam.backgroundColor = Color.Lerp(bgColor, Color.black, t);
            if (pCam.backgroundColor.Equals(Color.black))
            {
                spawnSet = 3;
            }
        }

        // XBOX Pause/Restart
        if (Input.GetKeyDown("joystick button 7")){ 
            /*isPause = !isPause;
            if(isPause)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;*/
            SceneManager.LoadScene("la-3");
        }

        if(transform.position.y < deathDepth)
        {
            deathAudio.clip = deathSound;
            deathAudio.Play();
            transform.parent.gameObject.transform.parent.transform.position = spawnPosition;
        }
    }

    // Pause menu GUI
    void OnGUI()
    {
        if(isPause)
            GUI.Window(0, MainMenu, TheMainMenu, "Pause Menu");
    }
    void TheMainMenu (int windowID) {
        if(GUILayout.Button("Reset")){
            isPause = !isPause;
            Time.timeScale = 1;
            SceneManager.LoadScene("la-3");
         }
    }
}