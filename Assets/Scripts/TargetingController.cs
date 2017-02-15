using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (LineRenderer))]
public class TargetingController : MonoBehaviour
{
	[Range(5.0f, 100f)]
    public float maxTargetRange = 25f;
    public Material outlineMaterial; //TODO: Write up a nicer outline shader
    #if UNITY_STANDALONE_WIN
        //current shader width values need to change depending on object scale/screen resolution?
        //platform specific compilation directive
        //outlineMaterial.SetFloat("_Outline", 0.3;);
    #endif
    public GameObject currentTarget; //no static
	private Material defaultMaterial;
    private Renderer currentRenderer;

    //Prototyping Stuff
    private Vector3 spawnPosition;

    void Start()
    {
        //Note: script should be in a child to the player character's camera object
        //Repsawn for prototyping
        spawnPosition = transform.parent.gameObject.transform.parent.transform.position;
    }

    void FixedUpdate()
    {
		//Repsawn for prototyping
        if (Input.GetKey(KeyCode.K) | transform.position.y < -100)
        {
            transform.parent.gameObject.transform.parent.transform.position = spawnPosition;
        }
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
        // Material[] materialArray = currentRenderer.materials;
        // defaultMaterial = materialArray[0];
        // materialArray[1] = outlineMaterial;
        // currentRenderer.materials = materialArray;
        // currentTarget = targetInteractable;
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
        currentRenderer.material = defaultMaterial;
        currentTarget = null;
    }
}

