﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (LineRenderer))]
public class TargetingController : MonoBehaviour
{
	[Range(10.0f, 100f)]
    public float maxTargetRange = 40f;
    public Material outlineMaterial; //TODO: Write up a nicer outline shader
    #if UNITY_STANDALONE_WIN
        //current shader width values need to change depending on object scale/screen resolution?
        //platform specific compilation directive
        //outlineMaterial.SetFloat("_Outline", 0.3;);
    #endif
    public GameObject currentTarget; //no static
	private Material defaultMaterial;
    private Renderer currentRenderer;

    void FixedUpdate()
    {
        //Clear Target if out of range -- Removing when it leaves FoV doesn't feel fun/gud
        if (currentTarget != null && (currentTarget.transform.position - transform.position).magnitude > maxTargetRange)
        {
            ClearTarget(currentTarget);
        }
    }

    /// <summary>
    /// Highlights targetInteractable by changing Renderer material to outlineMaterial and sets curerntTarget object
    /// </summary>
    /// <param name="targetInteractable"></param>
    public void SelectTarget(GameObject targetInteractable)
    {
        if (currentRenderer != null)
        {
            currentRenderer.material = defaultMaterial;
        }
        currentRenderer = targetInteractable.GetComponent<Renderer>();

        defaultMaterial = currentRenderer.material;
        currentRenderer.material = outlineMaterial;
        currentTarget = targetInteractable;
    }

    /// <summary>
    /// Clear the current target from lock on, remove visual indication and unset currentTarget
    /// </summary>
    /// <param name="targetInteractable"></param>
    public void ClearTarget(GameObject targetInteractable)
    {
        currentRenderer = targetInteractable.GetComponent<Renderer>();
        if (targetInteractable.tag.Equals("LevelFloor"))
        {
            defaultMaterial = (Material)Resources.Load("Materials/Prototyping/Glass", typeof(Material));
        }
        currentRenderer.material = defaultMaterial;
        currentTarget = null;
    }
}

