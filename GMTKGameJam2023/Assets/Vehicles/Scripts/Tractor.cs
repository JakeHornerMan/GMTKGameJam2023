using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractor : Car
{
    [Header("Tractor Logic")]
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private Transform[] sheepSpawnSpots;

    private void Start()
    {
        SetCarSpeed();

        soundManager.PlayNewTractor();
    }
}
