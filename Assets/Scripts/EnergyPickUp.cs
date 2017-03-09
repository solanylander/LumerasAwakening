using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickUp : MonoBehaviour {

    private ResourceManager resourceManager;
    private AudioSource audioSource;
    public AudioClip pickUpSound;

	void Start () {
        resourceManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ResourceManager>();
        audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[1];
    }
    void Update ()
    {
<<<<<<< HEAD
        transform.Rotate(Vector3.right, 90.0f * Time.deltaTime);
=======
        transform.Rotate(Vector3.up, 90.0f * Time.deltaTime);
>>>>>>> ff62596c7a5b6e2e44ff612af4f1e361ff69cb4f
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Contains("Player"))
        {
            audioSource.clip = pickUpSound;
            audioSource.Play();
            resourceManager.setResource(resourceManager.maxResource);
            gameObject.SetActive(false);
        }
    }
}
