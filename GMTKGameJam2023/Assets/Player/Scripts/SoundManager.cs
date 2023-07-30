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

    [Header("Game Info Clips")]
    [SerializeField] private SoundConfig gameSpeedConfig;
    [SerializeField] private SoundConfig lastSecondsConfig;

    [Header("Chicken Noise Clips")]
    [SerializeField] private SoundConfig[] chickenConfigs;

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

    public void PlayChickenHit()
    {
        RandomPlaySound(deathConfigs);
    }

    public void PlayNewStandardCar()
    {
        PlaySound(standardCarConfig);
    }

    public void PlayNewFastCar()
    {
        PlaySound(fastCarConfig);
    }

    public void PlayNewSpikeCar()
    {
        PlaySound(spikeCarConfig);
    }

    public void PlayNewTruck()
    {
        PlaySound(truckConfig);
    }

    public void PlayGameSpeed()
    {
        PlaySound(gameSpeedConfig);
    }

    public void PlayLastSeconds()
    {
        PlaySound(lastSecondsConfig);
    }

    public void PlayRandomChicken()
    {
        RandomPlaySound(chickenConfigs);
    }

    public void PlayRandomSlice()
    {
        RandomPlaySound(sliceConfigs);
    }

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
        if(willPlaySound == 0){
            if (soundConfigs.Length > 0)
            {
                int randomIndex = Random.Range(0, soundConfigs.Length);
                PlaySound(soundConfigs[randomIndex]);
            }
        }
    }
}
