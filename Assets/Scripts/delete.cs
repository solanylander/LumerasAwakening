using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class delete : MonoBehaviour {

    public GameObject player;
    public int deleteTime;
    private int count;
    private float walkSpeed, runSpeed, SensX, SensY;
    private AudioSource[] playerAudio;
    private AudioSource[] powerAudio;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        powerAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();

        walkSpeed = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed;
        runSpeed = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed;
        SensX = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.XSensitivity;
        SensY = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity = 0;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.XSensitivity = 0;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 0;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 0;

        player.GetComponent<CharacterController>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().spellEffect.SetActive(false);
        foreach (AudioSource aud in playerAudio)
        {
            aud.enabled = false;
        }
        foreach (AudioSource aud in powerAudio)
        {
            aud.enabled = false;
        }

        //TODO disable power controller
    }
	
	// Update is called once per frame
	void FixedUpdate () {
       //TODO map to controller Y lol
        if (Input.GetKeyDown(KeyCode.Y) | Input.GetButtonDown("YBut"))
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity = SensY;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.XSensitivity = SensX;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = runSpeed;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = walkSpeed;

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().enabled = true;
            player.GetComponent<CharacterController>().enabled = true;
            foreach (AudioSource aud in playerAudio)
            {
                aud.enabled = true;
            }
            foreach (AudioSource aud in powerAudio)
            {
                aud.enabled = true;
            }

            Destroy(gameObject);

        }
	}
}
