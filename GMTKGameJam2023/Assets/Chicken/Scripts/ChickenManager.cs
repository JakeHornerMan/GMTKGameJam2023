using System.Collections;
using System.Linq;
using UnityEngine;

public class ChickenManager : MonoBehaviour
{
    private float chickenCount;
    private float chickenAudioFrequency = 10f; // Starting interval, adjust as needed
    private Coroutine playChickenSoundsCoroutine;

    private Transform normalChickenContainer;
    private Transform specialChickenContainer;
    [SerializeField] private float killDelay = 0.05f; // Delay between each chicken kill

    // Reference to the sound manager, make sure to set this in the Inspector
    public SoundManager soundManager;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        
        if (GameObject.Find("ChickenContainer") != null)
            normalChickenContainer = GameObject.Find("ChickenContainer").transform;
        if (GameObject.Find("SpecialChickenContainer") != null)
            specialChickenContainer = GameObject.Find("SpecialChickenContainer").transform;

        // Start the coroutine to play chicken sounds
        if (soundManager != null)
        {
            playChickenSoundsCoroutine = StartCoroutine(PlayChickenSounds());
        }
        
    }

    void UpdateChickenCount()
    {
        chickenCount = transform.childCount;
        UpdateChickenAudioFrequency();
    }

    void UpdateChickenAudioFrequency()
    {
        // Adjust these values to control the ramping and leveling off behavior
        float maxFrequency = 3f; // Maximum frequency to level off at
        float rampUpSpeed = 0.04f; // How quickly the frequency ramps up

        // Calculate the frequency based on the number of chickens
        chickenAudioFrequency = maxFrequency * (1f - Mathf.Exp(-rampUpSpeed * chickenCount));

        if (chickenAudioFrequency < 0.5f)
        {
            chickenAudioFrequency = 0.5f;
        }
    }

    IEnumerator PlayChickenSounds()
    {
        while (true)
        {
            // Wait for a random time between 0 and chickenAudioFrequency seconds
            float waitTime = Random.Range(0.2f, chickenAudioFrequency);
            yield return new WaitForSeconds(waitTime);

            // Play a random chicken sound
            soundManager.PlayRandomChicken();
        }
    }

    void OnTransformChildrenChanged()
    {
        // Update the chicken count and audio frequency whenever the children of the transform change
        UpdateChickenCount();
    }

    public void StopAllChickens()
    {
        ChickenHealth[] allChickens = MakeChickenList();

        for (int i = 0; i < allChickens.Length; i++)
        {
            if (allChickens[i] == null) continue;
            allChickens[i].GetComponent<ChickenMovement>().stopMovement = true; // Disable the chicken movement
        }
    }

    public void WipeAllChickens()
    {
        StartCoroutine(WipeThemOut());
    }

    private IEnumerator WipeThemOut()
    {

        Egg[] allEggs = MakeEggList();
        ChickenHealth[] allChickens = MakeChickenList();

        killDelay = 1.5f / allChickens.Length;

        for (int i = 0; i < allEggs.Length; i++)
        {
            if (allEggs[i] == null) continue;
            allEggs[i].Hatch();
            yield return new WaitForSecondsRealtime(killDelay);
        }

        allChickens = MakeChickenList();

        for (int i = 0; i < allChickens.Length; i++)
        {
            if (allChickens[i] == null) continue;
            allChickens[i].TakeDamage(1000);
            yield return new WaitForSecondsRealtime(killDelay);
        }
    }

    private ChickenHealth[] MakeChickenList()
    {
        // Combine normal and special chickens
        ChickenHealth[] normalChickens = normalChickenContainer.GetComponentsInChildren<ChickenHealth>();
        ChickenHealth[] specialChickens = specialChickenContainer.GetComponentsInChildren<ChickenHealth>();
        ChickenHealth[] allChickens = specialChickens.Concat(normalChickens).ToArray();

        return allChickens;
    }

    private Egg[] MakeEggList()
    {
        Egg[] allEggs = normalChickenContainer.GetComponentsInChildren<Egg>();
        return allEggs;
    }
}