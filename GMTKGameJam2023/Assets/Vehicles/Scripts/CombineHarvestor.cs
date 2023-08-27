using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineHarvestor : Car
{
    private void Start()
    {
        SetCarSpeed();

        soundManager.PlayNewHarvestor();
    }
}
