using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooChooCounter : MonoBehaviour {

    private int maxChooChoos;
    public int collectedChooChoos;
    private Text chooChooText;

    // Use this for initialization
    void Start () {
        chooChooText = GetComponent<Text>();
        maxChooChoos = GameObject.FindGameObjectsWithTag("Choochoo").Length;
        collectedChooChoos = 0;
    }
	
	// Update is called once per frame
	void Update () {
        chooChooText.text = collectedChooChoos + "/" + maxChooChoos + " ChooChoos";
        if (collectedChooChoos.Equals(maxChooChoos))
        {
            chooChooText.text = "WOOHOO CHOOCHOO!!! :)";
        }
    }
}
