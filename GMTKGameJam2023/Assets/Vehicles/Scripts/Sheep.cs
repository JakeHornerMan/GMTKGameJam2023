using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Car
{
    [Header("References")]
    [SerializeField] private Collider2D sheepCollider;

    [Header("Effects")]
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float woolParticleDestroyDelay = 2f;

    [Header("Layers")]
    [SerializeField] private LayerMask waterLayer;

    [Header("Movement")]
    [SerializeField] private Vector2 carSpeedRange = new(1, 5);

    [Header("Settings")]
    [SerializeField] private bool allowSheepOnWater = false;

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

        // Death particles
        GameObject woolParticles = Instantiate(
            deathParticles,
            transform.position,
            Quaternion.identity
        );
        // Mark Particles for Destruction
        Destroy(woolParticles, woolParticleDestroyDelay);

        // Destroy after hitting chicken
        Destroy(gameObject);
    }

    protected override void HandleSheepCollision(Sheep sheep)
    {
        // Do Nothing - Other cars deal with this
    }
}
