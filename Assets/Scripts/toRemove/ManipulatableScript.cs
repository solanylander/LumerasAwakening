using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulatableScript : MonoBehaviour {
    public float baseWeight;
    //public float baseSize;
    public float multiplierModifier;
    // Use this for initialization
    public Rigidbody rb;

    void Start () {
        multiplierModifier = 1.0f;
        rb = GetComponent<Rigidbody>();
    }
    void Update() {
    }
        // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetButton("Fire1") && multiplierModifier > .02f)
        {
            multiplierModifier -= .01f;
            transform.localScale -= new Vector3(0.015f, 0.015f, 0.015f);
            rb.mass *= 0.97059014792f;
        }
        else if (Input.GetButton("Fire2") && multiplierModifier < 2.0f)
        {
            multiplierModifier += .01f;
            transform.localScale += new Vector3(0.015f, 0.015f, 0.015f);
            rb.mass *= 1.0303f;
        }
        else {
            if (multiplierModifier > 1.0f)
            {
                multiplierModifier -= .005f;
                transform.localScale -= new Vector3(0.0075f, 0.0075f, 0.0075f);
                rb.mass /= (1.005f * 1.005f * 1.005f);
            }
            else if (multiplierModifier < 1.0f)
            {
                multiplierModifier += .005f;
                transform.localScale += new Vector3(0.0075f, 0.0075f, 0.0075f);
                rb.mass *= (1.005f * 1.005f * 1.005f);
            }
            //Might need an else Statement?
        }
	}
}
