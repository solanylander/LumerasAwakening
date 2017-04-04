using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Teleporter : MonoBehaviour
{

    public Transform teleporterEnd;
    private static float useDelay;
    private static float nextUse;

    private IEnumerator coroutine;
    private GameObject fadeOut;
    private Color targetColor;
    private Color originalColor;
    private float duration = 0.2f; 
    private float fakeUpdate = 0.01f;
    private AudioSource[] playerAudio;
    private AudioSource[] powerAudio;

    void Start()
    {
        fadeOut = GameObject.FindGameObjectWithTag("TeleFade");
        originalColor = fadeOut.GetComponent<Image>().color;
        originalColor.a = 0.0f;
        targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        powerAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>();
        useDelay = 2.5f;
        nextUse = Time.time;
    }

    void OnTriggerEnter(Collider col)
    {
        if (Time.time >= nextUse && col.gameObject.tag.Contains("Player") && teleporterEnd != null)
        {
            coroutine = fade();
            StartCoroutine(coroutine);
            nextUse = Time.time + useDelay;
            col.gameObject.transform.position = teleporterEnd.position;
            //play teleport sound
            GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator fade()
    {
        float t = 0; 
        float lerpInc = fakeUpdate / duration; 
        while (t < 1)
        {
            fadeOut.GetComponent<Image>().color = Color.Lerp(fadeOut.GetComponent<Image>().color, targetColor, t);
            t += lerpInc;
            yield return new WaitForSeconds(fakeUpdate);
        }

        t = 0;
        lerpInc = fakeUpdate / duration;
        while (t < 1)
        {
            fadeOut.GetComponent<Image>().color = Color.Lerp(targetColor, originalColor, t);
            t += lerpInc;
            yield return new WaitForSeconds(fakeUpdate);
        }
        fadeOut.GetComponent<Image>().color = originalColor;
    }
}
