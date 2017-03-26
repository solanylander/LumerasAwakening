using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public Transform menu;
    public bool isPaused;

	// Use this for initialization
	void Start () {
        isPaused = false;
        menu.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            Pause();
        }
        else if(Input.GetKeyDown(KeyCode.R)) {
            if(isPaused) {
                isPaused = false;
                Time.timeScale = 1;
                menu.gameObject.SetActive(false);
            }
        }
        else if(Input.GetKeyDown(KeyCode.T)) {
            if(isPaused) {
                isPaused = false;
                Time.timeScale = 1;
                menu.gameObject.SetActive(false);
            }
        }

	}

    void Pause() {
        if(isPaused) {
            menu.gameObject.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }

        else {
            menu.gameObject.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
    }
}
