using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class achieved : MonoBehaviour {
    public int puzzle;
    bool done;
    float variable;

	// Use this for initialization
	void Start () {
        done = false;
        if (puzzle == 1)
        {
            variable = transform.localPosition.y;
        }
        if (puzzle == 2)
        {
            variable = transform.localScale.z;
        }
        if (puzzle == 3)
        {
            variable = transform.localScale.y;
        }
        if (puzzle == 4)
        {
            variable = transform.localScale.x;
        }

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!done)
        {
            if (puzzle == 1 && variable != transform.localPosition.y)
            {
                GetComponent<AudioSource>().Play();
                done = true;
            }
            if (puzzle == 2 && variable != transform.localScale.z)
            {
                GetComponent<AudioSource>().Play();
                done = true;
            }
            if (puzzle == 3 && variable != transform.localScale.y)
            {
                GetComponent<AudioSource>().Play();
                done = true;
            }
        }
	}
}
