using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCar : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager.PlayNewFastCar();
    }
}
