using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractor : Car
{
    [Header("Tractor Logic")]
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private Transform[] sheepSpawnSpots;
    [SerializeField][Range(0, 100)] private int sheepSpawnChance = 40;

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
            float rand = Random.Range(0, 100);
            if (rand < sheepSpawnChance)
                Instantiate(sheepPrefab, spot.position, Quaternion.identity);
        }
    }
}
