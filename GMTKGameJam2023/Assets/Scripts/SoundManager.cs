using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip death1, death2, carMove, powerUp, gameSpeed, chicken1, chicken2, chicken3, chicken4, truck, fastCar, slice1, slice2, newSpikeCar;

    static AudioSource audioSrc;

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
        NewSpikeCar
    }

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Function Called by Other Scripts
    public void PlaySound(SoundType soundType)
    {
        switch (soundType) 
        {
            case SoundType.Death:
                int rando = Random.Range(1, 2);
                if (rando == 2)
                    audioSrc.PlayOneShot(death1, 0.4f);
                else
                    audioSrc.PlayOneShot(death2, 0.4f);
                break;
            case SoundType.NewCar:
                audioSrc.PlayOneShot(carMove, 0.4f);
                break;
            case SoundType.GameSpeed:
                audioSrc.PlayOneShot(gameSpeed, 0.5f);
                break;
            case SoundType.ChickenNoise:
                int rando1 = Random.Range(1, 5);
                if (rando1 == 1) audioSrc.PlayOneShot(chicken1, 0.5f);
                if (rando1 == 2) audioSrc.PlayOneShot(chicken2, 0.5f);
                if (rando1 == 3) audioSrc.PlayOneShot(chicken3, 0.5f);
                if (rando1 == 4) audioSrc.PlayOneShot(chicken4, 0.5f);
                break;
            case SoundType.Truck:
                audioSrc.PlayOneShot(truck, 0.5f);
                break;
            case SoundType.FastCar:
                audioSrc.PlayOneShot(fastCar, 0.3f);
                break;
            case (SoundType.Slice):
                int rando2 = Random.Range(1, 3);
                if (rando2 == 1) audioSrc.PlayOneShot(slice1, 0.5f);
                if (rando2 == 2) audioSrc.PlayOneShot(slice2, 0.5f);
                break;
            case (SoundType.NewSpikeCar):
                audioSrc.PlayOneShot(newSpikeCar, 0.3f);
                break;
        }
    }
}
