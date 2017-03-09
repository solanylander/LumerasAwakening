using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformWidth : MonoBehaviour
{

    float initWidth, initHeight, counter;
    public int axis;

    // Use this for initialization
    void Start()
    {
        initWidth = transform.localScale.z;
        initHeight = transform.localScale.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter++;
        if (counter < 198)
        {
            if (axis == 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, initWidth * ((200.0f - counter) / 200));
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, initHeight * ((200.0f - counter) / 200), transform.localScale.z);
            }
        }
        else if (counter == 396)
        {
            counter = 0;
        }
        else
        {
            if (axis == 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, initWidth * ((counter - 196.5f) / 200));
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, initHeight * ((counter - 196.5f) / 200), transform.localScale.z);
            }
        }

    }
}