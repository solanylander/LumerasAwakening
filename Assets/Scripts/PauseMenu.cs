using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseMenu : MonoBehaviour {
    public Transform menu;
    private bool isPaused;
    private FirstPersonController playerController;
    private AudioSource[] playerAudio;
    private AudioSource[] powerAudio;


    void Start () {
        isPaused = false;
        menu.gameObject.SetActive(false);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        powerAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();
    }
	
	void Update () {
        //Reload from menu
        if (isPaused && (Input.GetKeyDown(KeyCode.Y) | Input.GetButtonDown("YBut"))) {
            UnPause();
            //TODO: Update with final main scene -- or main menu?
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        //TODO: update this to use the start button on the controller also
        if ((Input.GetKeyDown(KeyCode.P) | Input.GetButtonDown("StartBut")) && !isPaused) {
            Pause();
        } else if ((Input.GetKeyDown(KeyCode.P) | Input.GetButtonDown("StartBut")) && isPaused)
        {
            UnPause();
        }


        //These are just to make sure nothing breaks when we keyboard override respawns / restarts
        else if(Input.GetKeyDown(KeyCode.R)) {
            if(isPaused) {
                UnPause();
            }
        }
        else if(Input.GetKeyDown(KeyCode.T)) {
            if(isPaused) {
                UnPause();
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            if (isPaused)
            {
                UnPause();
            }
        }

    }

    void Pause() {
        menu.gameObject.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        playerController.enabled = false;

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().spellEffect.SetActive(false);
        foreach (AudioSource aud in playerAudio)
        {
            aud.enabled = false;
        }
        foreach (AudioSource aud in powerAudio)
        {
            aud.enabled = false;
        }
    }

    void UnPause()
    {
        menu.gameObject.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        playerController.enabled = true;

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().enabled = true;
        foreach (AudioSource aud in playerAudio)
        {
            aud.enabled = true;
        }
        foreach (AudioSource aud in powerAudio)
        {
            aud.enabled = true;
        }
    }
}
