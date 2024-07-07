using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTruck : Car, ISpecialLaunch
{
    [SerializeField] private GameObject fireNozzle;

    public override void Start()
    {
        base.Start();
        SetCarSpeed(carSpeed);
    }

    public void PerformSpecialLaunch()
    {
        fireNozzle.GetComponentInChildren<BoxCollider2D>().enabled = false;
    }




}
