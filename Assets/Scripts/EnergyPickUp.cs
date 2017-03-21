using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickUp : MonoBehaviour {

    private ResourceManager resourceManager;
    private AudioSource audioSource;
    public AudioClip pickUpSound;
    public GameObject particleEffect;

	void Start () {
        resourceManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ResourceManager>();
        audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[1];
    }
    void Update ()
    {
        //transform.Rotate(Vector3.right, 90.0f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Contains("Player"))
        {
            audioSource.clip = pickUpSound;
            audioSource.Play();
            resourceManager.setResource(resourceManager.maxResource);
            gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("ChooChooCounter").GetComponent<ChooChooCounter>().collectedChooChoos += 1;
            if (particleEffect != null) {
                particleEffect.gameObject.SetActive(false);
            }
        }
    }
}
