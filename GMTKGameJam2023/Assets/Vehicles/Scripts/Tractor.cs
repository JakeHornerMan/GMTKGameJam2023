using System;
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

        SpawnSheep();

        soundManager.PlayNewTractor();
    }

    private void SpawnSheep()
    {
        foreach (Transform spot in sheepSpawnSpots)
        {
            Instantiate(sheepPrefab, spot.position, Quaternion.identity);
        }
    }
}
