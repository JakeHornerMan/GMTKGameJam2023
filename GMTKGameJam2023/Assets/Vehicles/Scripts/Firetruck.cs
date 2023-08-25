using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetruck : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager.PlayNewFireTruck();
    }
}
