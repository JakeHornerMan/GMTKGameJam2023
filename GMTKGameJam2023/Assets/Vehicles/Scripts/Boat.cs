using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager?.PlayNewBoat();
    }
}
