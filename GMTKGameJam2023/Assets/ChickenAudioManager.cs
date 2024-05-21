using System.Collections;
using UnityEngine;

public class ChickenAudioManager : MonoBehaviour
{
    private float chickenCount;
    private float chickenAudioFrequency = 10f; // Starting interval, adjust as needed
    private Coroutine playChickenSoundsCoroutine;

    // Reference to the sound manager, make sure to set this in the Inspector
    public SoundManager soundManager;

    void Start()
    {
        // Start the coroutine to play chicken sounds
        playChickenSoundsCoroutine = StartCoroutine(PlayChickenSounds());
    }

    void UpdateChickenCount()
    {
        chickenCount = transform.childCount;
        UpdateChickenAudioFrequency();
    }

    void UpdateChickenAudioFrequency()
    {
        // Adjust these values to control the ramping and leveling off behavior
        float maxFrequency = 2f; // Maximum frequency to level off at
        float rampUpSpeed = 0.5f; // How quickly the frequency ramps up

        // Calculate the frequency based on the number of chickens
        chickenAudioFrequency = maxFrequency * (1f - Mathf.Exp(-rampUpSpeed * chickenCount));
    }

    IEnumerator PlayChickenSounds()
    {
        while (true)
        {
            // Wait for a random time between 0 and chickenAudioFrequency seconds
            float waitTime = Random.Range(0f, chickenAudioFrequency);
            yield return new WaitForSeconds(waitTime);

            // Play a random chicken sound
            SoundManager.instance.PlayRandomChicken();
        }
    }

    void OnTransformChildrenChanged()
    {
        // Update the chicken count and audio frequency whenever the children of the transform change
        UpdateChickenCount();
    }
}