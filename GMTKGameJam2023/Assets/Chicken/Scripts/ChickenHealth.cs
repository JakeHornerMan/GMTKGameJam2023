using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem featherParticles;
    [SerializeField] private float featherParticlesZPos = -5;

    [Header("Chicken Health Values")]
    [SerializeField] private int startHealth = 100;

    [Header("Scoring")]
    [SerializeField] public int pointsReward = 100;

    [HideInInspector] public int health = 0;

    private void Start()
    {
        health = startHealth;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
            HandleDeath();
    }

    private void HandleDeath()
    {
        Vector3 particlePos = new Vector3(transform.position.x, transform.position.y, featherParticlesZPos);
        Instantiate(featherParticles, particlePos, Quaternion.identity);
        Destroy(gameObject);
    }
}
