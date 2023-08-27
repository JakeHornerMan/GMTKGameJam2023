using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombChickenHealth : ChickenHealth
{
    [Header("References")]
    [SerializeField] private GameObject explosionPrefab;

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    protected override void HandleDeath()
    {
        Instantiate(
            explosionPrefab,
            transform.position,
            Quaternion.identity
        );
        soundManager.PlayGenericExplosion();
        base.HandleDeath();
    }
}
