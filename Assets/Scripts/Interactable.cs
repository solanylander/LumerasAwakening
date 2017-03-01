
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
    [Range(0.25f, 20.0f)]
    public float decayDelay;
    [Range(100.0f, 3000.0f)]
    public float decayRate;
    [RangeAttribute(0, 1)]
    public int enableDecay;

    private Vector3 originalScale;
    private bool disableScale = true; //For cases where scaling should be disabled completely
    private enum objectMaterial {Wood, Metal, Stone, Glass, Crystal, MoonStone};
    private float beginDecay;
    private bool decayable;

    private ColorGenerator colorGenerator;
    [SerializeField]
    private Renderer happyRenderer;

    //TODO: may be some bugs with update scale with anchored objects -- need to check
    void Start()
    {
        happyRenderer = gameObject.tag.Contains("Anchored") ? GetComponentInParent<Renderer>() : GetComponent<Renderer>();
        if (gameObject.transform.childCount >= 1)
        {
            happyRenderer = gameObject.transform.GetChild(0).gameObject.tag.Contains("Anchored") ?  gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>() : GetComponent<Renderer>();
        }
        originalScale = transform.localScale;
        beginDecay = float.PositiveInfinity;
        decayable = enableDecay == 0 ? false : true;
        colorGenerator = GameObject.FindGameObjectWithTag("ColorGenerator").GetComponent<ColorGenerator>();
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
        if (Time.time < beginDecay && !transform.localScale.Equals(originalScale) && decayable)
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) > 0)
            {
                float duration = (beginDecay - Time.time) * 5000;
                //float duration = (float)Math.Pow((beginDecay - Time.time), (beginDecay - Time.time)) * 100;
                float t = Mathf.PingPong(Time.time, duration) / duration;
                happyRenderer.material.SetColor("_Color", Color.Lerp(happyRenderer.material.color, colorGenerator.interactableColor, t));
            } 
        } else if (Time.time > beginDecay && !transform.localScale.Equals(originalScale) && decayable)
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position,transform.position) > 0) // Only trigger decay once player has moved away from object a given distance
            {
                lerpScaleDecay();
            } else
            {
                beginDecay = Time.time + decayDelay;
            }
        }
    }

    /// <summary>
    /// Scale this object given updated scale vector. Note: currently this is updated in PowerController TODO: Refactor/Update from PP1
    /// </summary>
    /// <param name="newScale"></param>
    /// <param name="triggerDecay"></param>
    public void updateScale(Vector3 newScale, bool triggerDecay){
        happyRenderer.material.SetColor("_Color", colorGenerator.selectedColor);
        transform.localScale = newScale;
        if (triggerDecay)
        {   
            beginDecay = Time.time + decayDelay; //TODO: This should not add full decay delay length when object is scaled at max scale?
        } else
        {
            beginDecay = float.PositiveInfinity;
        }
    }

    /// <summary>
    /// Lerp between the altered scale and original scale of the object based on decayRate
    /// </summary>
    /// <remarks>
    /// TODO: Update the color to represent the remaining time until decay
    /// </remarks>
    private void lerpScaleDecay()
    {
        happyRenderer.material.SetColor("_Color", colorGenerator.interactableColor);
        float t = Mathf.PingPong(Time.time, decayRate) / decayRate;
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, t);
    }

    private void lerpDecayColor()
    {
        //Renderer currentRenderer = GetComponent<Renderer>();
        //currentRenderer.material.SetColor("_Color", Color.Lerp(colorGenerator.selectedColor, colorGenerator.interactableColor, decayDelay));
        //float duration = 3000f;
        //float t = Mathf.PingPong(Time.time, duration) / duration;
        ////currentRenderer.material.color = Color.Lerp(colorGenerator.selectedColor, colorGenerator.interactableColor, t);
        //Material selectedMaterial = (Material)Resources.Load("Materials/Prototyping/Selected", typeof(Material));
        //selectedMaterial.color = Color.Lerp(colorGenerator.selectedColor, colorGenerator.interactableColor, t);
    }

    public void shrink()
    {
        beginDecay -= decayDelay;
    }
}
