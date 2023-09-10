using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardCar : Car
{
    void Start()
    {
        SetCarSpeed();

        soundManager?.PlayNewStandardCar();
    }
}
