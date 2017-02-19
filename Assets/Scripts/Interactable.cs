
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
    [Range(0.25f, 10.0f)]
    public float decayDelay;
    [Range(1.0f, 1000.0f)]
    public float decayRate;
    [RangeAttribute(0, 1)]
    public int enableDecay;

    private Vector3 originalScale;
    private bool disableScale = true; //For cases where scaling should be disabled completely
    private enum objectMaterial {Wood, Metal, Stone, Glass, Crystal, MoonStone};
    private float beginDecay;
    private bool decayable;

    //TODO: may be some bugs with update scale with anchored objects -- need to check
    void Start()
    {
        originalScale = transform.localScale;
        beginDecay = float.PositiveInfinity;
        decayable = enableDecay == 0 ? false : true;
    }

    void Update()
    {
        //Temp testing triggers
        if (this.tag.Contains("LevelFloor"))
        {
            if (Math.Max(transform.localScale.x, Math.Max(transform.localScale.y, transform.localScale.z)) >= maxScale && disableScale)
            {
                this.tag = "LevelFloor";
                this.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Prototyping/Glass", typeof(Material));
                disableScale = false;
            }
        }

        if (Time.time > beginDecay && !transform.localScale.Equals(originalScale) && decayable)
        {
            lerpScaleDecay();
        }
    }

    /// <summary>
    /// Scale this object given updated scale vector. Note: currently this is updated in PowerController TODO: Refactor/Update from PP1
    /// </summary>
    /// <param name="newScale"></param>
    /// <param name="triggerDecay"></param>
    public void updateScale(Vector3 newScale, bool triggerDecay){
        transform.localScale = newScale;
        if (triggerDecay)
        {
            beginDecay = Time.time + decayDelay;
        } else
        {
            beginDecay = float.PositiveInfinity;
        }
    }

    /// <summary>
    /// Lerp between the altered scale and original scale of the object based on decayRate
    /// </summary>
    private void lerpScaleDecay()
    {
        float t = Mathf.PingPong(Time.time, decayRate) / decayRate;
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, t);
    }
}
