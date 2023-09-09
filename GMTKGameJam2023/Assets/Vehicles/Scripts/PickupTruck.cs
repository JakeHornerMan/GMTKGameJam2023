using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTruck : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager?.PlayNewPickupTruck();
    }
}
