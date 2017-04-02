using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    //TODO: Loading Bar or something to indicate something is happening
    private Text screenText;
    public GameObject fade;

    // Use this for initialization
    void Start () {
        screenText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("ABut") || Input.GetKeyDown(KeyCode.A)) {
            //screenText.text = "Loading!";
            //StartCoroutine(fadeOut());
            SceneManager.LoadScene("Scenes/the-end");
        }
    }

    private IEnumerator fadeOut()
    {
        fade.GetComponent<Image>().color = Color.Lerp(fade.GetComponent<Image>().color, new Color(0,0,0,255) , 3f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scenes/the-end");
    }


    }
