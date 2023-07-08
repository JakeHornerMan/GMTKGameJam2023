using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip death1, death2, carMove, powerUp;

    static AudioSource audioSrc;

    public enum SoundType
    {
        Death,
        NewCar,
        PowerUp
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
            case SoundType.PowerUp:
                audioSrc.PlayOneShot(powerUp, 0.5f);
                break;
        }
    }
}
