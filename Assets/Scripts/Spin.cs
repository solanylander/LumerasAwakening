using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    bool up;
    float x, nextHeight, yRotation;

    public GameObject next;

    // Use this for initialization
    void Start()
    {
        x = 0.0f;
        up = false;
        nextHeight = next.transform.position.y;
        yRotation = transform.localRotation.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(yRotation != transform.localRotation.y)
        {
            Destroy(transform.GetComponent<CapsuleCollider>());
            Destroy(transform.GetComponent<MeshRenderer>());
            up = true;
        }
        if (up)
        {
            if(next.transform.position.y < nextHeight + 7.0f)
            {
                next.transform.position = new Vector3(next.transform.position.x, next.transform.position.y + 0.1f, next.transform.position.z);
            }else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            x += 0.01f;
            if (x >= 1.5f)
            {
                x = 0.0f;
            }
            transform.localRotation = new Quaternion(x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
        }
    }
}
