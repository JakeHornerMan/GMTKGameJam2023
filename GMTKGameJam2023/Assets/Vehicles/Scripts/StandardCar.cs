using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardCar : Car
{
    [Header("Standard Car Properties")]
    [SerializeField] private SoundConfig[] spawnSound;

    void Start()
    {
        SetCarSpeed();

        soundManager?.RandomPlaySound(spawnSound);
    }
}
