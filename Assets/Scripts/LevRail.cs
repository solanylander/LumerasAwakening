using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevRail : MonoBehaviour {

	private Renderer objectRenderer;
	private Material activeMaterial;
	private Material deactiveMaterial;
	public GameObject triggerNode;  //beam node with event trigger

	private PrismTest beamNode;

	public bool reverseNodeLogic;

	void Start () {
		objectRenderer = GetComponent<Renderer>();
		activeMaterial = (Material)Resources.Load("Materials/Prototyping/LevRailActive", typeof(Material));
		deactiveMaterial = (Material)Resources.Load("Materials/Prototyping/LevRailDeactive", typeof(Material));
		beamNode = triggerNode.GetComponent<PrismTest>();
	}
	
	void Update () {
		if (!reverseNodeLogic) {
			if (beamNode.beamActive) {
				Activate();
			} else {
				Deactivate();
			}
		} else {
			if (!beamNode.beamActive) {
				Activate();
			} else {
				Deactivate();
			}
		}
	}

	public void Deactivate() {
		//float duration = (100 - Time.time) * 5000;
		float duration = 350;
		//float duration = (float)Math.Pow((beginDecay - Time.time), (beginDecay - Time.time)) * 100;
    	float t = Mathf.PingPong(Time.time, duration) / duration;
    	//happyRenderer.material.SetColor("_Color", Color.Lerp(happyRenderer.material.color, colorGenerator.interactableColor, t));
		objectRenderer.material.Lerp(objectRenderer.material, deactiveMaterial, t);
	}

	public void Activate() {
		float duration = 100;
		float t = Mathf.PingPong(Time.time, duration) / duration;
		objectRenderer.material.Lerp(objectRenderer.material, activeMaterial, t);
	}
}
