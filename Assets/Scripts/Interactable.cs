
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable: MonoBehaviour
{	
    //Param Script gameObject.getComponent<Interactable>.maxScale, etc.
    [Range(0.01f, 150.0f)]
    public float maxScale;

	[Range(0.01f, 50.0f)]
    public float minScale;

    [Range(0.01f, 100.0f)]
    public float maxMass;
    
    [Range(0.01f, 1.0f)]
    public float minMass;

    private bool disableScale = true; //For cases where scaling should be disabled completely

    private enum objectMaterial {Wood, Metal, Stone, Glass, Crystal, MoonStone};
	
    void Update()
    {
        if (this.tag.Contains("LevelFloor")) {
            if (Math.Max(transform.localScale.x, Math.Max(transform.localScale.y, transform.localScale.z)) >= maxScale && disableScale)
            {
                this.tag = "LevelFloor";
                this.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Prototyping/Glass", typeof(Material));
                disableScale = false;
            } 
        }
    }
	/// <summary>
    /// Scale this object given updated scale vector. Note: currently this is updated in PowerController TODO: Refactor/Update from PP1
    /// </summary>
    /// <param name="newScale"></param>
    public void updateScale(Vector3 newScale){
        transform.localScale = newScale; 
    }
}
