using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public  AudioClip death1, death2, carMove;
    static AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>(); //find audio source component
    }

    public void PlaySound(string sound) //function called in other sctipts to play sound
    {
        switch (sound) 
        {
            case "death":
                int rando = Random.Range(1, 2);
                if(rando == 2){
                    audioSrc.PlayOneShot(death1, 0.4f);
                }
                else{
                    audioSrc.PlayOneShot(death2, 0.4f);
                }
                break;
            case "newCar":
                audioSrc.PlayOneShot(carMove, 0.4f);
                break;
        }
    }


}
