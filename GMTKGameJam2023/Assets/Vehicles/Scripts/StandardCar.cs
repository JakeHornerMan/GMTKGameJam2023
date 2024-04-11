using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardCar : Car
{
    public override void Start()
    {
        base.Start();
        SetCarSpeed(carSpeed);
    }
}
