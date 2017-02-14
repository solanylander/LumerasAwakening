
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable: MonoBehaviour
{	
    //Param Script gameObject.getComponent<Interactable>.maxScale, etc.
    [Range(1.0f, 10.0f)]
    public float maxScale;

	[Range(1.0f, 10.0f)]
    public float minScale;

    [Range(1.0f, 10.0f)]
    public float maxMass;
    
    [Range(1.0f, 10.0f)]
    public float minMass; 
    void Start(){

    }

    void FixedUpdate(){

    }

	/// <summary>
    /// Scale this object given updated scale vector. Note: currently this is updated in PowerController TODO: Refactor/Update from PP1
    /// </summary>
    /// <param name="newScale"></param>
    public void updateScale(Vector3 newScale){
        transform.localScale = newScale; 
    }
}
