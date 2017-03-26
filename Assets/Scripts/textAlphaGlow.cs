using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textAlphaGlow : MonoBehaviour {
    Text text;
    bool glow;
    Color color;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        glow = false;
        color = text.color;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (glow == false) 
            color.a -= 0.02f;
        else 
            color.a += 0.02f;
        text.color = color;
        if (color.a <= 0.0f)
            glow = true;
        else if (color.a >= 1.0f)
            glow = false;
	}
}
