using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private float cementLifetime = 4f;

    private int cementSpawned = 0;

    public override void Start()
    {
        base.Start();

        SetCarSpeed(carSpeed);

        cementSpawned = 0;

        InvokeRepeating(nameof(InstantiateCement), initalCementDelay, cementSpawnInterval);
    }

    private void InstantiateCement()
    {
        if (carInAction == true)
        {
            if (cementSpawned >= numCementToSpawn)
            {
                CancelInvoke(nameof(InstantiateCement));
                return;
            }

            GameObject newCement = Instantiate(
                cementPrefab,
                cementSpawnPos.position,
                Quaternion.Euler(0, 0, Random.Range(0, 360))
            );

            soundManager.PlayCementPour();

            Destroy(newCement, cementLifetime);

            cementSpawned++;
        }
        
    }
}
