using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturedRock : MonoBehaviour
{
    public GameObject triggerObject;

    void Update()
    {
        if (!triggerObject.activeSelf)
        {
            this.gameObject.GetComponent<Renderer>().enabled = true;
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActiveRecursively(true);
            }
        }
    }
}
