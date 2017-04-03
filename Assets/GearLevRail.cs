using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearLevRail : MonoBehaviour
{

    private Renderer objectRenderer;
    private Renderer objectRenderert;
    private Material activeMaterial;
    private Material deactiveMaterial;
    public GameObject one, two;
    float height;

    public bool reverseNodeLogic;

    void Start()
    {
        height = transform.position.y;
        objectRenderer = one.GetComponent<Renderer>();
        objectRenderert = two.GetComponent<Renderer>();
        activeMaterial = (Material)Resources.Load("Materials/Prototyping/LevRailActive", typeof(Material));
        deactiveMaterial = (Material)Resources.Load("Materials/Prototyping/LevRailDeactive", typeof(Material));
    }

    void Update()
    {

        if (height == transform.position.y)
        {
            Deactivate();
        }else
        {
            Activate();
        }
        height = transform.position.y;
    }

    public void Deactivate()
    {
        //float duration = (100 - Time.time) * 5000;
        float duration = 550;
        //float duration = (float)Math.Pow((beginDecay - Time.time), (beginDecay - Time.time)) * 100;
        float t = Mathf.PingPong(Time.time, duration) / duration;
        //happyRenderer.material.SetColor("_Color", Color.Lerp(happyRenderer.material.color, colorGenerator.interactableColor, t));
        objectRenderer.material.Lerp(objectRenderer.material, deactiveMaterial, t);
        objectRenderert.material.Lerp(objectRenderer.material, deactiveMaterial, t);
    }

    public void Activate()
    {
        float duration = 100;
        float t = Mathf.PingPong(Time.time, duration) / duration;
        objectRenderer.material.Lerp(objectRenderer.material, activeMaterial, t);
        objectRenderert.material.Lerp(objectRenderer.material, deactiveMaterial, t);
    }
}
