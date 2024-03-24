using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class SoundConfig
{
    [SerializeField] public AudioClip clip;
    [SerializeField][Range(0f, 1f)] public float volume = 1f;
}

public class SoundManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string musicObjectName = "Intro_Game_Menu_Theme";

    [Header("Clicken Death Clips")]
    [SerializeField] private SoundConfig[] deathConfigs;

    [Header("Player Attack Clips")]
    [SerializeField] private SoundConfig[] sliceConfigs;
    [SerializeField] private SoundConfig missileLaunchConfig;
    [SerializeField] private SoundConfig fighterJetExplosion;
    [SerializeField] private SoundConfig[] explosionConfigs;
    [SerializeField] private SoundConfig spikePlaceConfig;
    [SerializeField] private SoundConfig cementPlaceConfig;
    [SerializeField] private SoundConfig sheepDeathConfig;

    [Header("Game Info Clips")]
    [SerializeField] private SoundConfig gameSpeedConfig;
    [SerializeField] private SoundConfig timesUpConfig;
    [SerializeField] private SoundConfig missedChicken;
    [SerializeField] private SoundConfig[] tokenCollect;
    [SerializeField] private SoundConfig[] purchases;
    [SerializeField] private SoundConfig cantPurchase;
    [SerializeField] private SoundConfig freezeConfig;
    [SerializeField] private SoundConfig unfrostConfig;

    [Header("Chicken Noise Clips")]
    [SerializeField] private SoundConfig[] chickenConfigs;
    [SerializeField] private SoundConfig[] cashChickenSound;
    [SerializeField] private SoundConfig turboChicken;
    [SerializeField] private SoundConfig wagonChicken;
    [SerializeField] private SoundConfig turboChickenDeath;
    [SerializeField] private SoundConfig wagonChickenDeath;
    [SerializeField] private SoundConfig enterPortal;
    [SerializeField] private SoundConfig exitPortal;

    [Header("Wave Sounds")]
    [SerializeField] private SoundConfig lastSecondsConfig;

    [Header("Game Music")]
    [SerializeField] private AudioClip endMusic;
    [SerializeField] private AudioClip gameMusic;

    private AudioSource audioSrc;
    private AudioSource musicAudio;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        musicAudio = GameObject.Find(musicObjectName).GetComponent<AudioSource>();
    }

    private void Start()
    {
        musicAudio.Stop();
        audioSrc.clip = gameMusic;
        audioSrc.loop = false;
        audioSrc.Play();
    }

    // =============================
    // Player Attack Sounds
    public void PlayRandomSlice() => RandomPlaySound(sliceConfigs);
    public void PlayMissileLaunch() => PlaySound(missileLaunchConfig);
    public void PlayFighterJetExplosion() => PlaySound(fighterJetExplosion);
    public void PlayGenericExplosion() => RandomPlaySound(explosionConfigs);
    public void PlaySpikePlacement() => PlaySound(spikePlaceConfig);
    public void PlayCementPour() => PlaySound(cementPlaceConfig);
    public void PlaySheepDeath() => PlaySound(sheepDeathConfig);

    // =============================
    // Game Info Sounds
    public void PlayGameSpeed() => PlaySound(gameSpeedConfig);
    public void PlayTimesUp() => PlaySound(timesUpConfig);
    public void PlayMissedChicken() => PlaySound(missedChicken);
    public void PlayPurchase() => RandomPlaySound(purchases);
    public void PlayCantPurchase() => PlaySound(cantPurchase);
    public void PlayTokenCollect() => RandomPlaySound(tokenCollect);
    public void PlayFreeze() => PlaySound(freezeConfig);
    public void PlayUnfrost(float time) => PlaySound(time, unfrostConfig);

    // =============================
    // Chicken SFX
    public void PlayRandomChicken() => RandomPlaySound(chickenConfigs);
    public void PlayChickenHit() => RandomPlaySound(deathConfigs);
    public void PlayCashChicken() => RandomPlaySound(cashChickenSound);
    public void PlayTurboChicken() => PlaySound(turboChicken);
    public void PlayWagonChicken() => PlaySound(wagonChicken);
    public void PlayTurboChickenDeath() => PlaySound(turboChickenDeath);
    public void PlayWagonChickenDeath() => PlaySound(wagonChickenDeath);
    public void PlayEnterPortal() => PlaySound(enterPortal);
    public void PlayExitPortal() => PlaySound(exitPortal);

    // =============================
    // Wave Sounds
    public void PlayLastSeconds() => PlaySound(lastSecondsConfig);


    // =============================
    // Other Functions
    public void PlayEndMusic()
    {
        audioSrc.Stop();
        musicAudio.PlayOneShot(endMusic, 0.05f);
    }

    private void PlaySound(SoundConfig soundConfig)
    {
        // if (!Settings.sfxAllowed) return;
        if (soundConfig == null) return;
        audioSrc.PlayOneShot(soundConfig.clip, soundConfig.volume);
    }

    public void RandomPlaySound(params SoundConfig[] soundConfigs)
    {
        // if (!Settings.sfxAllowed) return;
        int willPlaySound = Random.Range(0, 1);
        if (willPlaySound == 0)
        {
            if (soundConfigs.Length > 0)
            {
                int randomIndex = Random.Range(0, soundConfigs.Length);
                PlaySound(soundConfigs[randomIndex]);
            }
        }
    }

    public void PlaySound(float time, SoundConfig soundConfig)
    {
        StartCoroutine(DelaySound(time, soundConfig));
    }

    public IEnumerator DelaySound(float time, SoundConfig soundConfig)
    {
        yield return new WaitForSeconds(time);
        PlaySound(soundConfig);
    }

    public void PlayWaveSound(string input){
        switch (input)
        {
            case "LastSeconds":
                PlayLastSeconds();
                break;
            default:
                // Debug.Log("Input not recognised for wave sound.");
                break;
        }
    }
}

