using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedCar : Car, ISpecialLaunch
{
    [SerializeField] private GameObject[] spikes;
    public override void Start()
    {
        base.Start();
        SetCarSpeed(carSpeed);
    }

    public void PerformSpecialLaunch()
    {
        for (int i = 0; i < spikes.Length; i++)
        {
            spikes[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
