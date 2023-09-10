using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Car
{
    [Header("References")]
    [SerializeField] private Collider2D sheepCollider;

    [Header("Layers")]
    [SerializeField] private LayerMask waterLayer;

    [Header("Movement")]
    [SerializeField] private Vector2 carSpeedRange = new(1, 5);

    [Header("Settings")]
    [SerializeField] private bool allowSheepOnWater = false;

    private void Start()
    {
        soundManager?.PlaySheepNoise();
    }

    private void Update()
    {
        carSpeed = Random.Range(carSpeedRange.x, carSpeedRange.y);
        SetCarSpeed();

        if (!allowSheepOnWater && sheepCollider.IsTouchingLayers(waterLayer))
        {
            Destroy(gameObject);
        }
    }

    public override void HandleChickenCollision(ChickenHealth chickenHealth)
    {
        base.HandleChickenCollision(chickenHealth);

        HandleDeath();
    }

    public void HandleDeath()
    {
        soundManager.PlaySheepDeath();

        // TODO Death particles

        // Destroy after hitting chicken
        Destroy(gameObject);
    }

    protected override void HandleSheepCollision(Sheep sheep)
    {
        // Do Nothing - Other cars deal with this
    }
}
