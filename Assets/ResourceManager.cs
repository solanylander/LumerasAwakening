using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {

    public int maxResource = 100;
    public int currentResource;

    public Slider resourceSlider; // Reference to the UI's health bar.
    public float flashSpeed = 5f; // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f); // The colour the damageImage is set to, to flash.                                                           
    public Image damageImage; // Reference to an image to flash on the screen on being hurt

    private bool damaged; // True when the player gets damaged.

    void Start () {
        currentResource = maxResource;
	}
	
	void FixedUpdate () {
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            //damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void decrementResource(int amount)
    {
        damaged = true;
        currentResource -= amount;
        resourceSlider.value = currentResource;
        if (currentResource <= 0)
        {
            //Something, die
            Debug.Log("Resource is Zero");
        }
    }
}