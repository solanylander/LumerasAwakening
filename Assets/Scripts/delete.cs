﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delete : MonoBehaviour {

    public GameObject player;
    public int deleteTime;
    int count;
    float walkSpeed, runSpeed, SensX, SensY;

	// Use this for initialization
	void Start ()
    {
        walkSpeed = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed;
        runSpeed = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed;
        SensX = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.XSensitivity;
        SensY = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity = 0;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.XSensitivity = 0;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 0;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.anyKey)
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity = SensY;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.XSensitivity = SensX;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = runSpeed;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = walkSpeed;
            Destroy(gameObject);

        }
	}
}