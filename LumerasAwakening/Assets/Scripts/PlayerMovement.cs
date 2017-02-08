using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody rb;
    GameObject[] adjustables;
    List<GameObject> inRange;
    GameObject currObject = null;
    int i;
    Renderer rend;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
        adjustables = GameObject.FindGameObjectsWithTag("adjustable");
        inRange = new List<GameObject>();
        i = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
        // Slow down time on button press (Left CTRL)
        if (Input.GetButton("Fire1")) {
            Time.timeScale = 0.2F;
            findObjectsInRadius();

            // If there are objects in range
            if(inRange.Count != 0) {
                if (i >= inRange.Count)
                        i = 0;
                currObject = inRange[i];
                rend = currObject.GetComponent<Renderer>();
                rend.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");

                // Switch between objects (Left SHIFT)
                if (Input.GetButtonDown("Fire3")) {
                    i++;
                    rend.material.shader = Shader.Find("Diffuse");
                    if (i >= inRange.Count)
                        i = 0;
                    currObject = inRange[i];
                    Debug.Log(currObject);
                    rend = currObject.GetComponent<Renderer>();
                    rend.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                }

                // Perform action on chosen object (Left ALT)
                if (Input.GetButtonDown("Fire2")) {
                    Debug.Log(currObject);
                    currObject.SetActive(false);
                }
            }
        }

        // Regular time speed
        else 
            Time.timeScale = 1.0F;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        this.transform.Translate(Input.GetAxis("Horizontal")*Time.timeScale*0.2f,0,0);
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
	}

    // Mechanic for preventing jumping mid-air
    void OnCollisionStay(Collision collision) {
        if((collision.gameObject.tag == "ground" || collision.gameObject.tag == "adjustable") && Input.GetButtonDown("Jump")) {
            rb.AddForce (0, 600, 0);
        }
    }

    // Puts objects in range in a list for use
    void findObjectsInRadius() {
        foreach (GameObject ob in adjustables) {
            float distanceSqr = (transform.position - ob.transform.position).sqrMagnitude;

            // Object in range
            if (distanceSqr < 20) {
                if (!inRange.Contains(ob)) {
                    inRange.Add(ob);
                }
            }

            // Object not in range
            else {
                inRange.Remove(ob);
                ob.SetActive(true);
                rend = ob.GetComponent<Renderer>();
                rend.material.shader = Shader.Find("Diffuse");
            }
        }
    }
}