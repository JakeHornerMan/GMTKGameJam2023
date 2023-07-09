using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : Car
{
    [Header("Camera Shake")]
    [SerializeField] private float truckShakeDuration = 3f;
    [SerializeField] private float truckShakeMagnitude = 0.25f;

    void Start()
    {
        SetCarSpeed();

        // Shake Camera
        StartCoroutine(cameraShaker.Shake(truckShakeDuration, truckShakeMagnitude));
    }
}
