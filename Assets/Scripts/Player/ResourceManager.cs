using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {

    public int maxResource = 100;
    public float currentResource;

    public Slider resourceSlider; // Reference to the UI's health bar.
    public float flashSpeed = 5f; // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f); // The colour the damageImage is set to, to flash.                                                           
    public Image damageImage; // Reference to an image to flash on the screen on being hurt
    public float regenTickAmount = 5; //Regeneration resource / second
    public float regenTickRate = 1;

    private float debugCurTime;
    private float nextRegenTick;
    public bool damaged; // True when the player gets damaged.

    public AudioClip nopeAudio;
    private AudioSource audioSource;
    private float nextNope;
    private bool lockedOut;

    private ColorGenerator colorGenerator;

    void Start () {
        currentResource = maxResource;
        nextRegenTick = 1.0f;
        audioSource = GetComponents<AudioSource>()[1];
        nextNope = 0.0f;
        colorGenerator = GameObject.FindGameObjectWithTag("ColorGenerator").GetComponent<ColorGenerator>();
        ColorBlock colorBlock = resourceSlider.colors;
        colorBlock.normalColor = colorGenerator.interactableColor;
        resourceSlider.colors = colorBlock;
    }

    void FixedUpdate() {
        debugCurTime = Time.time;
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
        if (Time.time > nextRegenTick && currentResource < maxResource)
        {
            nextRegenTick = Time.time + regenTickRate;
            decrementResource(-regenTickAmount);

            if (currentResource < 5.5f)
            {
                currentResource = 5.5f;
                nextRegenTick = Time.time + regenTickRate * 2; //Punish 
                playNope();
            }

        }
        resourceSlider.value = currentResource;
    }

    /// <summary>
    /// Decrement the player resource pool by specified amount, also used for regeneration with a negative float argument
    /// </summary>
    /// <remarks>
    /// Note: Tuning/Balancing this should take into consideration the power drain specified in power controller in addition to regen rates and tick amounts
    /// </remarks>
    /// <param name="amount"></param>
    public void decrementResource(float amount)
    {
        currentResource -= amount;
    }

    public void setResource(float amount)
    {
        currentResource = amount;
    }

    public void playNope()
    {
        if (Time.time > nextNope)
        {
            audioSource.Stop();
            audioSource.clip = nopeAudio;
            audioSource.Play();
            nextNope = Time.time + 1.5f; //TODO: balance this stuff
            GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().decrementScore(25);
        }
    }
}