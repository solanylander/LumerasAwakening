using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CutsceneTrigger : MonoBehaviour
{
    public Camera cutsceneCam;
    public GameObject cine;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Camera playerCam;
    public int time;
    [SerializeField]
    private bool played;
    private AudioSource[] playerAudio;
    private AudioSource[] powerAudio;

    void Start()
    {
        played = false;
        cine.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        playerCam = player.GetComponentInChildren<Camera>();
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        powerAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider collision)
    {
        if (!played)
        {
            played = true;
            cine.SetActive(true);
            //cutsceneCam.GetComponent<Animation>().Play("LaserEntrance");
            StartCoroutine(wait());

        }
    }

    IEnumerator wait()
    {
        playerCam.enabled = false;
        cutsceneCam.enabled = true;

        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().m_WalkSpeed = 0;
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().m_RunSpeed = 0;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().enabled = false;
        GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().spellEffect.SetActive(false);
        foreach (AudioSource aud in playerAudio)
        {
            aud.enabled = false;
        }
        foreach (AudioSource aud in powerAudio)
        {
            aud.enabled = false;
        }

        yield return new WaitForSeconds(time);
        playerCam.enabled = true;
        cutsceneCam.enabled = false;
        player.GetComponent<CharacterController>().enabled = true;

        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().m_WalkSpeed = 11;
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().m_RunSpeed = 11;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().enabled = true;
        GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>().enabled = true;
        foreach (AudioSource aud in playerAudio)
        {
            aud.enabled = true;
        }
        foreach (AudioSource aud in powerAudio)
        {
            aud.enabled = true;
        }
    }
}
