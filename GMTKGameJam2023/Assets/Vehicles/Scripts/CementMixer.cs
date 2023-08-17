using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CementMixer : Car
{
    [Header("References")]
    [SerializeField] private GameObject cementPrefab;
    [SerializeField] private Transform cementSpawnPos;

    [Header("Cement Values")]
    [SerializeField] private int numCementToSpawn = 10;
    [SerializeField] private float initalCementDelay = 1f;
    [SerializeField] private float cementSpawnInterval = 0.5f;

    private int cementSpawned = 0;

    private void Start()
    {
        SetCarSpeed();

        cementSpawned = 0;

        InvokeRepeating(nameof(InstantiateCement), initalCementDelay, cementSpawnInterval);
    }

    private void InstantiateCement()
    {
        if (cementSpawned >= numCementToSpawn)
        {
            CancelInvoke(nameof(InstantiateCement));
            return;
        }

        Instantiate(
            cementPrefab,
            cementSpawnPos.position,
            Quaternion.identity
        );

        cementSpawned++;
    }
}
