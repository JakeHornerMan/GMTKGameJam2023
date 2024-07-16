using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrecker : Car, ISpecialLaunch
{
    [SerializeField] private GameObject wreckingColliders;

    public override void Start()
    {
        base.Start();
        SetCarSpeed(carSpeed);
    }

    public void PerformSpecialLaunch()
    {
        Collider2D[] colliders = gameObject.GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }
}
