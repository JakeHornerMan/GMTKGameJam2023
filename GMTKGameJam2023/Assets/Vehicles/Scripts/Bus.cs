using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager.PlayNewBus();
    }
}
