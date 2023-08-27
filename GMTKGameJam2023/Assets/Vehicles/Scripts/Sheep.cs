using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sheep : Car
{
    [Header("References")]
    [SerializeField] private Collider2D sheepCollider;

    [Header("Layers")]
    [SerializeField] private LayerMask waterLayer;

    [Header("Settings")]
    [SerializeField] private bool allowSheepOnWater = false;

    private void Start()
    {
        SetCarSpeed();

        soundManager.PlaySheepNoise();
    }

    private void Update()
    {
        if (!allowSheepOnWater && sheepCollider.IsTouchingLayers(waterLayer))
        {
            Destroy(gameObject);
        }
    }

    public override void HandleChickenCollision(ChickenHealth chickenHealth)
    {
        base.HandleChickenCollision(chickenHealth);

        // Destroy after hitting chicken
        Destroy(gameObject);
    }
}
