﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEventTrigger : MonoBehaviour {

    private PrismTest prismTest;
    public GameObject triggerObject;
    public int displacement;
    [Range(0.05f, 1.0f)]
    public float displacementSpeed;
    private Vector3 originalPosition;
    private Vector3 endPosition;
    [Range(0, 1)]
    public int triggerOnActive;
    public bool firstCompletion;
    [SerializeField]
    private CameraTakeOver cameraTakeOver;
    private AudioSource[] playerAudio;

	void Start () {
        prismTest = GetComponent<PrismTest>();
        originalPosition = triggerObject.transform.position;
        endPosition = new Vector3(originalPosition.x, originalPosition.y + displacement, originalPosition.z);
        firstCompletion = false;
        cameraTakeOver = gameObject.GetComponent<CameraTakeOver>();
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
	}
	
	void FixedUpdate () {
        if (triggerObject.transform.position.Equals(endPosition) && !firstCompletion)
        {
            firstCompletion = true;
            if (cameraTakeOver != null)
            {
                cameraTakeOver.activate = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = true;
                foreach (AudioSource aud in playerAudio)
                {
                    aud.enabled = true;
                }
                if (GetComponents<AudioSource>()[1] != null)
                {
                    GetComponents<AudioSource>()[1].Stop();
                }
            }
        }

        if (triggerOnActive == 1)
        {
            if (prismTest.beamActive)
            {
                if (cameraTakeOver != null && !firstCompletion && Time.time > 10f)
                {
                    cameraTakeOver.activate = true;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = false;
                    foreach (AudioSource aud in playerAudio)
                    {
                        aud.enabled = false;
                    }
                    if (GetComponents<AudioSource>()[1] != null && !GetComponents<AudioSource>()[1].isPlaying)
                    {
                        GetComponents<AudioSource>()[1].Play();
                    }
                }
                //event to happen when activated
                triggerObject.transform.position = Vector3.MoveTowards(triggerObject.transform.position, endPosition, displacementSpeed);
            }
            else
            {
                triggerObject.transform.position = Vector3.MoveTowards(triggerObject.transform.position, originalPosition, displacementSpeed);
            }
        } else
        {
            if (!prismTest.beamActive)
            {
                if (cameraTakeOver != null && !firstCompletion && Time.time > 10f)
                {
                    cameraTakeOver.activate = true;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = false;
                    foreach (AudioSource aud in playerAudio)
                    {
                        aud.enabled = false;
                    }
                    if (GetComponents<AudioSource>()[1] != null && !GetComponents<AudioSource>()[1].isPlaying)
                    {
                        GetComponents<AudioSource>()[1].Play();
                    }
                }
                //event to happen when activated
                triggerObject.transform.position = Vector3.MoveTowards(triggerObject.transform.position, endPosition, displacementSpeed);
            }
            else
            {
                triggerObject.transform.position = Vector3.MoveTowards(triggerObject.transform.position, originalPosition, displacementSpeed);
            }
        }
	}
}
