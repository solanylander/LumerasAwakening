using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooChooCounter : MonoBehaviour {

    public int maxChooChoos;
    public int collectedChooChoos;
    //private Text chooChooText;

    public Sprite[] rockSprites;
    private Image rockImage;

    // Use this for initialization
    void Start () {
        //chooChooText = GetComponent<Text>();
        rockImage = GetComponent<Image>();
        maxChooChoos = GameObject.FindGameObjectsWithTag("Choochoo").Length;
        collectedChooChoos = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //chooChooText.text = collectedChooChoos + "/" + maxChooChoos + " Relics";
        //if (collectedChooChoos.Equals(maxChooChoos))
        //{
        //    chooChooText.text = "Woohoo!";
        //}
        rockImage.sprite = rockSprites[collectedChooChoos];
    }
}
