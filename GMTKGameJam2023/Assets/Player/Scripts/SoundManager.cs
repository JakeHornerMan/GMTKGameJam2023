using UnityEngine;

[System.Serializable]
public class SoundConfig
{
    [SerializeField] public AudioClip clip;
    [SerializeField][Range(0f, 1f)] public float volume = 1f;
}

public class SoundManager : MonoBehaviour
{
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

    [Header("Player Attack Clips")]
    [SerializeField] private SoundConfig missileLaunchConfig;
    [SerializeField] private SoundConfig explosionConfig;
    [SerializeField] private SoundConfig spikePlaceConfig;
    [SerializeField] private SoundConfig cementPlaceConfig;

    [Header("Game Info Clips")]
    [SerializeField] private SoundConfig gameSpeedConfig;
    [SerializeField] private SoundConfig lastSecondsConfig;
    [SerializeField] private SoundConfig missedChicken;
    [SerializeField] private SoundConfig[] tokenCollect;

    [Header("Chicken Noise Clips")]
    [SerializeField] private SoundConfig[] chickenConfigs;
    [SerializeField] private SoundConfig cashChicken;
    [SerializeField] private SoundConfig turboChicken;

    [Header("Slice Clips")]
    [SerializeField] private SoundConfig[] sliceConfigs;

    [Header("Game Music")]
    [SerializeField] private AudioClip endMusic;
    [SerializeField] private AudioClip gameMusic;

    private AudioSource audioSrc;
    private AudioSource musicAudio;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        musicAudio = GameObject.Find("Music").GetComponent<AudioSource>();
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

    // =============================
    // Player Attack Sounds
    public void PlayRandomSlice() => RandomPlaySound(sliceConfigs);

    // =============================
    // Game Info Sounds
    public void PlayGameSpeed() => PlaySound(gameSpeedConfig);
    public void PlayLastSeconds() => PlaySound(lastSecondsConfig);
    public void PlayMissedChicken() => PlaySound(missedChicken);

    // =============================
    // Chicken SFX
    public void PlayRandomChicken() => RandomPlaySound(chickenConfigs);
    public void PlayChickenHit() => RandomPlaySound(deathConfigs);
    public void PlayCashChicken() => PlaySound(cashChicken);


    // =============================
    // Other Functions
    public void PlayEndMusic()
    {
        audioSrc.Stop();
        musicAudio.PlayOneShot(endMusic, 0.05f);
    }

    private void PlaySound(SoundConfig soundConfig)
    {
        audioSrc.PlayOneShot(soundConfig.clip, soundConfig.volume);
    }

    private void RandomPlaySound(params SoundConfig[] soundConfigs)
    {
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
