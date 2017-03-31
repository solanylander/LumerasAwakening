using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnFromSandbox : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Contains("Player"))
        {
            //Other Stuff
            SceneManager.LoadScene("Scenes/the-end");
        }
    }
}
