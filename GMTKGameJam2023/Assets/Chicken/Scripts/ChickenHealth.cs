using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected ParticleSystem featherParticles;
    [SerializeField] protected float featherParticlesZPos = -5;
    [SerializeField] private GameObject chickenSprite;
    [SerializeField] private GameObject hopController;
    [HideInInspector] protected Animator anim;

    [Header("Settings")]
    [SerializeField] private bool damagedByHose = true;

    [Header("Tags")]
    [SerializeField] private string hoseWaterTag = "Hose Water";

    [Header("Chicken Health Values")]
    [SerializeField] private int startHealth = 100;
    [SerializeField] private float invinsibleForTime = 1f;

    [Header("Scoring")]
    [SerializeField] public int pointsReward = 100;

    [HideInInspector] public int health = 0;

    [Header("Damage Invinsible Flash Values")]
    [SerializeField] private Material flashMaterial;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    [HideInInspector] protected bool isInvinsible;

    private void Awake()
    {
        health = startHealth;
        spriteRenderer = chickenSprite.GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        anim = chickenSprite.GetComponent<Animator>();
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            HandleDeath();
        }
        else
        {
            HandleHit();
        }
    }

    protected virtual void HandleDeath()
    {
        Vector3 particlePos = new(transform.position.x, transform.position.y, featherParticlesZPos);
        Instantiate(featherParticles, particlePos, Quaternion.identity);
        Destroy(gameObject);
    }

    protected virtual void HandleHit()
    {
        if (!isInvinsible)
        {
            isInvinsible = true;
            StartCoroutine(StartInvinsibleTime());
        }
    }

    public IEnumerator StartInvinsibleTime()
    {
        hopController.GetComponent<BoxCollider2D>().enabled = false;
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(invinsibleForTime);
        spriteRenderer.material = originalMaterial;
        hopController.GetComponent<BoxCollider2D>().enabled = true;
        isInvinsible = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag(hoseWaterTag))
        {
            return;
        }
    }
}
