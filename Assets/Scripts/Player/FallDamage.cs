using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{

    private ResourceManager resourceManager;
    private CharacterController characterController;

    [SerializeField]
    private float lastYPos;
    [SerializeField]
    private float fallDistance;

    public float fallDamage = 10f;
    public float fallDamageThreshold = 5f;

    private AudioSource audioSource;
    public AudioClip ouchAudio;

    void Start()
    {
        audioSource = GetComponents<AudioSource>()[1];
        resourceManager = GetComponentInChildren<ResourceManager>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (lastYPos > characterController.gameObject.transform.position.y)
        {
            fallDistance += lastYPos - characterController.gameObject.transform.position.y;
        }
        lastYPos = characterController.gameObject.transform.position.y;
        if (fallDistance >= fallDamageThreshold && characterController.isGrounded)
        {
            audioSource.clip = ouchAudio;
            audioSource.Play();
            resourceManager.decrementResource(fallDamage);
            fallDistance = 0;
            lastYPos = 0;
        }
        else if (fallDistance <= fallDamageThreshold && characterController.isGrounded)
        {
            fallDistance = 0;
            lastYPos = 0;
        }
    }
}
