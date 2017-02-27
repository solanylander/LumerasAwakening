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
        transform.Rotate(Vector3.up, 90.0f * Time.deltaTime);
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
