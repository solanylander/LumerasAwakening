using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    //TODO: Loading Bar or something to indicate something is happening
    private Text screenText;

    // Use this for initialization
    void Start () {
        screenText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.anyKey) {
            screenText.text = "Loading";
            SceneManager.LoadScene("Scenes/the-end");
        }
    }


}
