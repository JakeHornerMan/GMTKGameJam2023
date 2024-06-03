using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script giving the tank its actual functionality.
/// (Movement of tank is handled by standard car movement script.)
/// Rotates cannon towards nearest special chicken, shoots bomb which spawns a small explosion.
///
/// Steps:
/// 1. Locate Nearest Special Chicken
/// 2. Aim at its location (don't track it)
/// 3. Spawn Projectile (rest handled by script on projectile)
/// </summary>
public class TankTargeting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cannonTransform;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Effects")]
    [SerializeField] private GameObject shootParticlePrefab;
    [SerializeField] private float particleDestroyDelay = 2f;

    [Header("Gameplay Settings")]
    [SerializeField] private float fireCooldown = 2.0f;
    [SerializeField] private float rotationSpeed = 5.0f;  // Speed at which the cannon rotates
    [SerializeField] private float activationDelay = 3.0f;  // Time before the cannon starts targeting

    private GameObject chickenContainer;
    private float fireCooldownTimer;
    private float activationDelayTimer;
    private GameObject currentTarget;

    private void Awake()
    {
        chickenContainer = GameObject.Find("SpecialChickenContainer");
        activationDelayTimer = activationDelay;
    }

    private void Update()
    {
        if (activationDelayTimer > 0)
        {
            activationDelayTimer -= Time.deltaTime;
            return;
        }

        UpdateCurrentTarget();

        if (currentTarget != null)
        {
            AimAtTarget(currentTarget.transform.position);

            fireCooldownTimer -= Time.deltaTime;
            if (fireCooldownTimer <= 0f)
            {
                FireProjectile();
                fireCooldownTimer = fireCooldown;
            }
        }
    }

    private void UpdateCurrentTarget()
    {
        if (currentTarget == null || !currentTarget.activeInHierarchy)
        {
            currentTarget = GetClosestSpecialChicken();
        }
    }

    private GameObject GetClosestSpecialChicken()
    {
        GameObject bestTarget = null;
        Vector3 currentPosition = transform.position;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (Transform child in chickenContainer.transform)
        {
            Vector3 directionToTarget = child.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = child.gameObject;
            }
        }
        return bestTarget;
    }

    private void AimAtTarget(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;  // Adjust angle if necessary
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        cannonTransform.rotation = Quaternion.Lerp(cannonTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FireProjectile()
    {
        TankProjectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<TankProjectile>();
        projectile.FireTo(currentTarget.transform.position);
        GameObject shootParticles = Instantiate(shootParticlePrefab, firePoint.position, firePoint.rotation);
        Destroy(shootParticles, particleDestroyDelay);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
