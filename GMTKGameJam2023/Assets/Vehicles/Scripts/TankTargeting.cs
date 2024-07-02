using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script giving the tank its actual functionality.
/// (Movement of tank is handled by standard car movement script.)
/// Rotates cannon towards nearest special chicken or normal chicken, shoots bomb which spawns a small explosion.
///
/// Steps:
/// 1. Locate Nearest Chicken (Special or Normal)
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
    [SerializeField] private float aimThresholdDegrees = 5f; // Threshold in degrees for considering aim complete
    private bool isAimingComplete = false;

    private GameObject specialChickenContainer;
    private GameObject normalChickenContainer;
    private List<Transform> allChickens;  // List to store all chickens
    private float fireCooldownTimer;
    private float activationDelayTimer;
    private GameObject currentTarget;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        specialChickenContainer = GameObject.Find("SpecialChickenContainer");
        normalChickenContainer = GameObject.Find("ChickenContainer");
        activationDelayTimer = activationDelay;
        fireCooldownTimer = 2f;
        InitializeChickenList();
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
            isAimingComplete = AimAtTarget(currentTarget.transform.position);

            fireCooldownTimer -= Time.deltaTime;
            if (fireCooldownTimer <= 0f && isAimingComplete)
            {
                FireProjectile();
                fireCooldownTimer = fireCooldown;

                InitializeChickenList();
            }
        }
    }

    // Initialize the list of all chickens
    private void InitializeChickenList()
    {
        allChickens = new List<Transform>();

        if (specialChickenContainer != null)
        {
            foreach (Transform child in specialChickenContainer.transform)
            {
                allChickens.Add(child);
            }
        }

        if (normalChickenContainer != null)
        {
            foreach (Transform child in normalChickenContainer.transform)
            {
                allChickens.Add(child);
            }
        }
    }

    private void UpdateCurrentTarget()
    {
        if (currentTarget == null || !currentTarget.activeInHierarchy)
        {
            currentTarget = GetClosestChicken();
        }
    }

    private GameObject GetClosestChicken()
    {
        GameObject bestTarget = null;
        Vector3 currentPosition = transform.position;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Transform chicken in allChickens)
        {
            if (chicken != null && chicken.gameObject.activeInHierarchy)  // Check if the chicken is active
            {
                Vector3 directionToTarget = chicken.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = chicken.gameObject;
                }
            }
        }

        return bestTarget;
    }

    private bool AimAtTarget(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        cannonTransform.rotation = Quaternion.Lerp(cannonTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Calculate the angle difference, considering the wraparound at 360 degrees
        float currentAngle = cannonTransform.rotation.eulerAngles.z;
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

        // Return true if the angle difference is within the threshold
        return angleDifference <= aimThresholdDegrees;
    }

    private void FireProjectile()
    {
        animator.SetTrigger("Kickback");
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
