using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombChickenHealth : ChickenHealth
{
    [Header("References")]
    [SerializeField] private GameObject explosionPrefab;

    protected override void HandleDeath()
    {
        Instantiate(
            explosionPrefab,
            transform.position,
            Quaternion.identity
        );
        base.HandleDeath();
    }
}
