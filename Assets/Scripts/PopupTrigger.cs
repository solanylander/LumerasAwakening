using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTrigger : MonoBehaviour {
    public GameObject popup;
    private bool spent;

	// Use this for initialization
	void Start () {
        spent = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Contains("Player") && !spent)
        {
            spent = true;
            popup.SetActive(true);
        }
    }
}
