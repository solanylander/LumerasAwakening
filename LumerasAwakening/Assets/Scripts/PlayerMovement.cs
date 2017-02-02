using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody rb;
    GameObject[] adjustables;
    List<GameObject> inRange;
    GameObject currObject = null;
    int i;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
        adjustables = GameObject.FindGameObjectsWithTag("adjustable");
        inRange = new List<GameObject>();
        i = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
        // Slow down time on button press
        if (Input.GetButton("Fire1")) {
            Time.timeScale = 0.2F;
            findObjectsInRadius();
            if(inRange != null) {
                if (i >= inRange.Count)
                        i = 0;
                currObject = inRange[i];
                Renderer rend = currObject.GetComponent<Renderer>();
                rend.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                if(Input.GetButtonDown("Fire3")) {
                    i++;
                    rend.material.shader = Shader.Find("Diffuse");
                    if (i >= inRange.Count)
                        i = 0;
                    currObject = inRange[i];
                    Debug.Log(currObject);
                    rend = currObject.GetComponent<Renderer>();
                    rend.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                }
                if (Input.GetButtonDown("Fire2")) {
                    Debug.Log(currObject);
                    rend.material.shader = Shader.Find("Diffuse");
                    currObject.SetActive(false);
                }
            }
        }

        else 
            Time.timeScale = 1.0F;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        this.transform.Translate(Input.GetAxis("Horizontal")*Time.timeScale*0.2f,0,0);
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
	}

    void OnCollisionStay(Collision collision) {

        if(collision.gameObject.tag == "ground" && Input.GetButtonDown("Jump")) {
            rb.AddForce (0, 300, 0);
        }
    }

    void findObjectsInRadius() {
        foreach (GameObject ob in adjustables) {
            float distanceSqr = (transform.position - ob.transform.position).sqrMagnitude;
            if (distanceSqr < 20) {
                if (!inRange.Contains(ob)) {
                    inRange.Add(ob);
                }
                //ob.SetActive(false);
            }
            else {
                inRange.Remove(ob);
                ob.SetActive(true);
            }
        }
    }
}