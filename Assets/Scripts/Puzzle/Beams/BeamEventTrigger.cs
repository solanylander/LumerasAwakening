using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

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
    private AudioSource[] powerAudio;

    private Vector3 currentPosition;

	void Start () {
        prismTest = GetComponent<PrismTest>();
        originalPosition = triggerObject.transform.position;
        endPosition = new Vector3(originalPosition.x, originalPosition.y + displacement, originalPosition.z);
        firstCompletion = false;
        cameraTakeOver = gameObject.GetComponent<CameraTakeOver>();
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        powerAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();
    }
	
	void FixedUpdate () {
        currentPosition = triggerObject.transform.position;
        //TODO: this is a TEMP workaround for the floating point rounding bug, figure out a more elegant and modular solution
        if (triggerObject.transform.position.y <= endPosition.y + 1f && !firstCompletion && Time.timeSinceLevelLoad > 10f)
        {
            firstCompletion = true;
            if (cameraTakeOver != null)
            {
                cameraTakeOver.activate = false;
                //GameObject.FindGameObjectWithTag("UI").SetActive(true);
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
                if (cameraTakeOver != null && !firstCompletion && Time.timeSinceLevelLoad > 10f)
                {
                    cameraTakeOver.activate = true;
                    //GameObject.FindGameObjectWithTag("UI").SetActive(false);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().m_WalkSpeed = 0;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().m_RunSpeed = 0;
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().enabled = false;
                    GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>().enabled = false;
                    foreach (AudioSource aud in playerAudio)
                    {
                        aud.enabled = false;
                    }
                    foreach (AudioSource aud in powerAudio)
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
                if (cameraTakeOver != null && !firstCompletion && Time.timeSinceLevelLoad > 10f)
                {
                    cameraTakeOver.activate = true;
                    //GameObject.FindGameObjectWithTag("UI").SetActive(false);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().m_WalkSpeed = 0;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().m_RunSpeed = 0;
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PowerController>().enabled = false;
                    GameObject.FindGameObjectWithTag("UITAG").GetComponent<Canvas>().enabled = false;
                    foreach (AudioSource aud in playerAudio)
                    {
                        aud.enabled = false;
                    }
                    foreach (AudioSource aud in powerAudio)
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
