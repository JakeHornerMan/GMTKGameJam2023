using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hovercraft : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager.PlayNewHovercraft();
    }
}
