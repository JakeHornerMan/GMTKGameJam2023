using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulldozer : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager?.PlayNewBulldozer();
    }
}
