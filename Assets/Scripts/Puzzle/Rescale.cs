using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescale : MonoBehaviour
{
    bool scaleDown;
    float previous;

    // Use this for initialization
    void Start()
    {
        scaleDown = false;
        previous = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.y > previous)
        {
            scaleDown = false;
        }
        if (transform.localScale.y > 1.0f && scaleDown)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.1f, transform.localScale.z - 0.1f);
        }else
        {
            scaleDown = false;
        }
        previous = transform.localScale.y;
    }

    void OnTriggerEnter(Collider other)
    {
        scaleDown = true;
    }
}
