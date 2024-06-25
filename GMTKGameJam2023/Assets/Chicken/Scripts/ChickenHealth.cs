using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected ParticleSystem featherParticles;
    [SerializeField] protected float featherParticlesZPos = -5;
    [SerializeField] public GameObject chickenSprite;
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
    [HideInInspector] private Color originalColor = Color.white;
    [SerializeField] private Color hurtColor;

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
        // Debug.Log("Damage :" +dmg + ", Health: " + health);
        if (health <= 0)
            HandleDeath();
        else
            HandleHit();
    }

    public IEnumerator ChipDamage(int damage)
    {
        TakeDamage(damage);

        if (health > 0)
        {
            yield return new WaitForSeconds(1f);
            RedoChipDamage(damage);
        }
    }

    private void RedoChipDamage(int damage)
    {
        StartCoroutine(ChipDamage(damage));
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
        spriteRenderer.color = hurtColor;
        yield return new WaitForSeconds(invinsibleForTime);
        spriteRenderer.color = originalColor;
        hopController.GetComponent<BoxCollider2D>().enabled = true;
        isInvinsible = false;
    }

    // void OnParticleTrigger()
    // {
    //     Debug.Log("Hit by particles");
    // }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log(other.gameObject.name);
    //    ParticleSystem ps = other.GetComponent<ParticleSystem>();
    //    if(ps != null) Debug.Log("Hit by particles");

    //}

    // void OnParticleCollision(GameObject other) {
    //     // Debug.Log ("Particle Collision!");
    //     Debug.Log(other.gameObject.name);
    // }

    public void FreezeChicken(float freezeLength)
    {
        IEnumerator coroutine;
        if (this.gameObject.name.Contains("Turbo") || this.gameObject.name.Contains("WheelBarrow"))
        {
            coroutine = this.gameObject.GetComponent<AlternativeChickenMovement>().FreezeChicken(freezeLength, true);
        }
        // else if (this.gameObject.name.Contains("WheelBarrow")){
        //     coroutine = this.gameObject.GetComponent<WagonChickenMovement>().FreezeChicken(freezeLength, true);
        // }
        else
        {
            coroutine = this.gameObject.GetComponent<ChickenMovement>().FreezeChicken(freezeLength, true);
        }
        // IEnumerator coroutine = chickenMovement.StopTheMovement(freezeLength, true);
        StartCoroutine(coroutine);
        // Debug.Log("Freezing this chciken: "+ this.gameObject.name 
        //     +". For seconds: "+ freezeLength);
    }

    public void FlashChicken(float stunLength)
    {
        IEnumerator coroutine;

        if (gameObject.TryGetComponent<AlternativeChickenMovement>(out AlternativeChickenMovement alternativeChickenMovement))
        {
            coroutine = alternativeChickenMovement.FlashChicken(stunLength, true);
        }
        else
        {
            coroutine = gameObject.GetComponent<ChickenMovement>().FlashChicken(stunLength, true);
        }
        StartCoroutine(coroutine);
    }
}
