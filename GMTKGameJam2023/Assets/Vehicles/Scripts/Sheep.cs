using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager.PlaySheepNoise();
    }
}
