using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSandbox : MonoBehaviour {
    public GameObject teleporterEffect;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindGameObjectWithTag("ChooChooCounter").GetComponent<ChooChooCounter>().collectedChooChoos >= GameObject.FindGameObjectWithTag("ChooChooCounter").GetComponent<ChooChooCounter>().maxChooChoos)
        {
            teleporterEffect.SetActive(true);
        } else
        {
            teleporterEffect.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Contains("Player") && GameObject.FindGameObjectWithTag("ChooChooCounter").GetComponent<ChooChooCounter>().collectedChooChoos >= GameObject.FindGameObjectWithTag("ChooChooCounter").GetComponent<ChooChooCounter>().maxChooChoos)
        {
            //Debug.Log(GameObject.FindGameObjectWithTag("ChooChooCounter").GetComponent<ChooChooCounter>().collectedChooChoos);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene("Scenes/funzone");
            GetComponent<AudioSource>().Play();
        }
    }
}
