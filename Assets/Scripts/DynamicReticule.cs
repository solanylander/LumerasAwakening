using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicReticule : MonoBehaviour {
	
    public Sprite[] reticuleSprites;
    private Image reticuleImage;
	private TargetingController targetController;
	private PowerController powerController; //This hover over should be moved to targeting but no time to worry about that now.
	private string objectType;
    void Start () 
	{
        reticuleImage = GetComponent<Image>();
		targetController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<TargetingController>();
		powerController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PowerController>();
    }
	
	/// <summary>
	/// Dynamically update the targeting reticule based on object scale direction
	/// </summary>
	void Update () 
	{	
		//Debug
		// if (targetController.currentTarget != null) {
		// 	Debug.Log(targetController.currentTarget.tag);
		// }	
		//OKAY GREAT WE CAN'T USE THE TAG B/C 3DS EXPORTS EVERYTHING SIDEWAYS AND THINGS ARE ROTATED RANDOMLY
		//USE THE MATERIAL SINCE WE COLOR CODE THAT MANUALLY HAHAHAHA

		if (powerController.currentMouseover != null && powerController.currentMouseover.GetComponent<Renderer>() != null) {
			//Debug.Log(powerController.lastMouseover.tag);
			//Debug.Log(powerController.currentMouseover.GetComponent<Renderer>().material.ToString());
			objectType = powerController.currentMouseover.GetComponent<Renderer>().material.ToString();
            //change reticule to correct sprite
            if (objectType.Contains("X"))
            {
                //Debug.Log("X");
                reticuleImage.sprite = reticuleSprites[1];
            }
            else if (objectType.Contains("Y"))
            {
                //Debug.Log("Y");
                reticuleImage.sprite = reticuleSprites[2];
            }
            else if (objectType.Contains("Z"))
            {
                //Debug.Log("Z");
                reticuleImage.sprite = reticuleSprites[3];
            }
            else if (powerController.currentMouseover.tag.Contains("Gear"))
            {
                reticuleImage.sprite = reticuleSprites[4];
            }
            else
            {
                reticuleImage.sprite = reticuleSprites[0];
            }
		} else {
			//null target, empty reticule
			reticuleImage.sprite = reticuleSprites[0];
		}
        //reticuleImage.sprite = reticuleSprites[i];
		
    }
}
