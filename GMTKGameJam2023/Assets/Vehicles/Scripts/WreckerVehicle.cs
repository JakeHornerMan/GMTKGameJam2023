using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckerVehicle : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager?.PlayNewWrecker();
    }
}
