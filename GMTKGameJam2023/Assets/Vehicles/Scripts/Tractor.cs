using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractor : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager.PlayNewTractor();
    }
}
