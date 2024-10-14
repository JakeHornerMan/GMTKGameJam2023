using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rigidbody based object, moves towards specified target when public function is run
/// Detonate Function spawns Small explosion when it is at the final position, and destroys itself.
/// </summary>
public class TankProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject smallExplosionPrefab;

    [Header("Gameplay Settings")]
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 target;
    public bool isFired = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void FireTo(Vector2 target)
    {
        this.target = target;
        isFired = true;
    }

    private void Update()
    {
        if (isFired)
        {
            Vector2 direction = (target - rb.position).normalized;
            rb.velocity = direction * moveSpeed;

            // Check if the projectile has reached the target
            if (Vector2.Distance(rb.position, target) <= 0.1f)
            {
                Detonate();
            }
        }
    }

    private void Detonate()
    {
        Instantiate(smallExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
