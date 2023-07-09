using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Clicken Death Clips")]
    [SerializeField] private AudioClip death1;
    [SerializeField] private AudioClip death2;

    [Header("Car Engine Clips")]
    [SerializeField] private AudioClip carMove;
    [SerializeField] private AudioClip fastCar;
    [SerializeField] private AudioClip newSpikeCar;

    [Header("Truck Clips")]
    [SerializeField] private AudioClip truck;

    [Header("Game Info Clips")]
    [SerializeField] private AudioClip powerUp;
    [SerializeField] private AudioClip gameSpeed;
    [SerializeField] private AudioClip lastSeconds;

    [Header("Chicken Noise Clips")]
    [SerializeField] private AudioClip chicken1;
    [SerializeField] private AudioClip chicken2;
    [SerializeField] private AudioClip chicken3;
    [SerializeField] private AudioClip chicken4;

    [Header("Slice Clips")]
    [SerializeField] private AudioClip slice1;
    [SerializeField] private AudioClip slice2;

    [Header("Game Music")]
    [SerializeField] private AudioClip endMusic;
    [SerializeField] private AudioClip gameMusic;

    static AudioSource audioSrc;
    public AudioSource musicAudio;

    public enum SoundType
    {
        Death,
        NewCar,
        PowerUp,
        GameSpeed,
        ChickenNoise,
        Truck,
        FastCar,
        Slice,
        NewSpikeCar,
        LastSeconds
    }

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        musicAudio = GameObject.Find("Music").GetComponent<AudioSource>();
        musicAudio.Stop();
        audioSrc.clip = gameMusic;
        audioSrc.loop = false;
        audioSrc.Play();
    }

    // Function Called by Other Scripts
    public void PlaySound(SoundType soundType)
    {
        switch (soundType) 
        {
            case SoundType.Death:
                RandomDeathNoise();
                break;
            case SoundType.NewCar:
                audioSrc.PlayOneShot(carMove, 0.4f);
                break;
            case SoundType.GameSpeed:
                audioSrc.PlayOneShot(gameSpeed, 0.3f);
                break;
            case SoundType.ChickenNoise:
                RandomChickenNoise();
                break;
            case SoundType.Truck:
                audioSrc.PlayOneShot(truck, 0.5f);
                break;
            case SoundType.FastCar:
                audioSrc.PlayOneShot(fastCar, 0.3f);
                break;
            case (SoundType.Slice):
                RandomSliceSound();
                break;
            case (SoundType.NewSpikeCar):
                audioSrc.PlayOneShot(newSpikeCar, 0.3f);
                break;
            case (SoundType.LastSeconds):
                audioSrc.PlayOneShot(lastSeconds, 0.2f);
                break;
        }
    }

    private void RandomDeathNoise()
    {
        int rando = Random.Range(1, 3);
        if (rando == 1) audioSrc.PlayOneShot(death1, 0.4f);
        else audioSrc.PlayOneShot(death2, 0.4f);
    }

    private void RandomSliceSound()
    {
        int rando = Random.Range(1, 3);
        if (rando == 1) audioSrc.PlayOneShot(slice1, 0.5f);
        if (rando == 2) audioSrc.PlayOneShot(slice2, 0.5f);
    }

    private void RandomChickenNoise()
    {
        int rando = Random.Range(1, 5);
        if (rando == 1) audioSrc.PlayOneShot(chicken1, 0.5f);
        if (rando == 2) audioSrc.PlayOneShot(chicken2, 0.5f);
        if (rando == 3) audioSrc.PlayOneShot(chicken3, 0.5f);
        if (rando == 4) audioSrc.PlayOneShot(chicken4, 0.5f);
    }

    public void PlayEndMuisc(){
        audioSrc.Stop();
        musicAudio.PlayOneShot(endMusic, 0.05f);
    }
}
