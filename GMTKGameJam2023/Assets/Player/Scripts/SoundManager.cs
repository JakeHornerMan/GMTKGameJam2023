using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SoundConfig
{
    [SerializeField] public AudioClip clip;
    [SerializeField][Range(0f, 1f)] public float volume = 1f;
}

public class SoundManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string musicObjectName = "Music";

    [Header("Clicken Death Clips")]
    [SerializeField] private SoundConfig[] deathConfigs;

    [Header("Car Engine Clips")]
    [SerializeField] private SoundConfig standardCarConfig;
    [SerializeField] private SoundConfig fastCarConfig;
    [SerializeField] private SoundConfig spikeCarConfig;
    [SerializeField] private SoundConfig truckConfig;
    [SerializeField] private SoundConfig pickupTruckConfig;
    [SerializeField] private SoundConfig[] fighterJetConfig;
    [SerializeField] private SoundConfig[] policeCarConfig;
    [SerializeField] private SoundConfig hovercraftConfig;
    [SerializeField] private SoundConfig cementMixerConfig;
    [SerializeField] private SoundConfig bulldozerConfig;
    [SerializeField] private SoundConfig busConfig;
    [SerializeField] private SoundConfig tractorConfig;
    [SerializeField] private SoundConfig boatConfig;
    [SerializeField] private SoundConfig fireTruckConfig;
    [SerializeField] private SoundConfig combineHarvestorConfig;
    [SerializeField] private SoundConfig wreckerConfig;

    [Header("Player Attack Clips")]
    [SerializeField] private SoundConfig[] sliceConfigs;
    [SerializeField] private SoundConfig missileLaunchConfig;
    [SerializeField] private SoundConfig fighterJetExplosion;
    [SerializeField] private SoundConfig[] explosionConfigs;
    [SerializeField] private SoundConfig spikePlaceConfig;
    [SerializeField] private SoundConfig cementPlaceConfig;
    [SerializeField] private SoundConfig sheepConfig;
    [SerializeField] private SoundConfig sheepDeathConfig;

    [Header("Game Info Clips")]
    [SerializeField] private SoundConfig gameSpeedConfig;
    [SerializeField] private SoundConfig lastSecondsConfig;
    [SerializeField] private SoundConfig missedChicken;
    [SerializeField] private SoundConfig[] tokenCollect;
    [SerializeField] private SoundConfig[] purchases;
    [SerializeField] private SoundConfig cantPurchase;

    [Header("Chicken Noise Clips")]
    [SerializeField] private SoundConfig[] chickenConfigs;
    [SerializeField] private SoundConfig[] cashChickenSound;
    [SerializeField] private SoundConfig turboChicken;
    [SerializeField] private SoundConfig turboChickenDeath;

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
    // Sounds Playing on Car Spawn
    public void PlayNewStandardCar() => PlaySound(standardCarConfig);
    public void PlayNewFastCar() => PlaySound(fastCarConfig);
    public void PlayNewSpikeCar() => PlaySound(spikeCarConfig);
    public void PlayNewTruck() => PlaySound(truckConfig);
    public void PlayNewPickupTruck() => PlaySound(pickupTruckConfig);
    public void PlayNewFighterJet() => RandomPlaySound(fighterJetConfig);
    public void PlayNewPoliceCar() => RandomPlaySound(policeCarConfig);
    public void PlayNewHovercraft() => PlaySound(hovercraftConfig);
    public void PlayNewCementMixer() => PlaySound(cementMixerConfig);
    public void PlayNewBulldozer() => PlaySound(bulldozerConfig);
    public void PlayNewBus() => PlaySound(busConfig);
    public void PlayNewTractor() => PlaySound(tractorConfig);
    public void PlayNewBoat() => PlaySound(boatConfig);
    public void PlayNewFireTruck() => PlaySound(fireTruckConfig);
    public void PlayNewHarvestor() => PlaySound(combineHarvestorConfig);
    public void PlayNewWrecker() => PlaySound(wreckerConfig);

    // =============================
    // Player Attack Sounds
    public void PlayRandomSlice() => RandomPlaySound(sliceConfigs);
    public void PlayMissileLaunch() => PlaySound(missileLaunchConfig);
    public void PlayFighterJetExplosion() => PlaySound(fighterJetExplosion);
    public void PlayGenericExplosion() => RandomPlaySound(explosionConfigs);
    public void PlaySpikePlacement() => PlaySound(spikePlaceConfig);
    public void PlayCementPour() => PlaySound(cementPlaceConfig);
    public void PlaySheepNoise() => PlaySound(sheepConfig);
    public void PlaySheepDeath() => PlaySound(sheepDeathConfig);

    // =============================
    // Game Info Sounds
    public void PlayGameSpeed() => PlaySound(gameSpeedConfig);
    public void PlayLastSeconds() => PlaySound(lastSecondsConfig);
    public void PlayMissedChicken() => PlaySound(missedChicken);
    public void PlayPurchase() => RandomPlaySound(purchases);
    public void PlayCantPurchase() => PlaySound(cantPurchase);
    public void PlayTokenCollect() => RandomPlaySound(tokenCollect);

    // =============================
    // Chicken SFX
    public void PlayRandomChicken() => RandomPlaySound(chickenConfigs);
    public void PlayChickenHit() => RandomPlaySound(deathConfigs);
    public void PlayCashChicken() => RandomPlaySound(cashChickenSound);
    public void PlayTurboChicken() => PlaySound(turboChicken);
    public void PlayTurboChickenDeath() => PlaySound(turboChickenDeath);

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
        audioSrc.PlayOneShot(soundConfig.clip, soundConfig.volume);
    }

    private void RandomPlaySound(params SoundConfig[] soundConfigs)
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
}

