using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCar : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager?.PlayNewSpikeCar();
    }
}
