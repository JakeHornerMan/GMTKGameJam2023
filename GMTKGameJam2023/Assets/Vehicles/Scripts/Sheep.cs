using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D sheepCollider;

    [Header("Tags")]
    [SerializeField] private string deathboxTag = "Death Box";

    [Header("Effects")]
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float woolParticleDestroyDelay = 2f;

    [Header("Layers")]
    [SerializeField] private LayerMask waterLayer;

    [Header("Movement")]
    [SerializeField] private Vector2 carSpeedRange = new(1, 5);

    [Header("Damage")]
    [SerializeField] private int damage = 120;

    [Header("Settings")]
    [SerializeField] private bool allowSheepOnWater = false;

    [Header("Camera Shake Values")]
    [SerializeField] private float camShakeDuration = 0.15f;
    [SerializeField] private float camShakeMagnitude = 0.05f;

    [Header("PopUp Values")]
    [SerializeField] private GameObject scorePopUp;
    [SerializeField] private string scorePopUpMsg = "";
    [Tooltip("Text after getting points, e.g. 100 {Poitns}")]
    [SerializeField] private float popupDestroyDelay = 0.7f;

    private Rigidbody2D rb;
    private CameraShaker cameraShaker;
    private SoundManager soundManager;

    private float carSpeed = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        soundManager = FindObjectOfType<SoundManager>();
        cameraShaker = FindObjectOfType<CameraShaker>();
    }

    private void Update()
    {
        carSpeed = Random.Range(carSpeedRange.x, carSpeedRange.y);

        if (rb != null)
            rb.velocity = transform.up * carSpeed;

        if (!allowSheepOnWater && sheepCollider.IsTouchingLayers(waterLayer))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if Hit Deathbox
        if (collision.gameObject.CompareTag(deathboxTag))
            HandleDeath();

        // Check if Hit Chicken
        ChickenHealth chickenHealth = collision.gameObject.GetComponent<ChickenHealth>();
        if (chickenHealth == null && collision.transform.parent != null)
            chickenHealth = collision.transform.parent.GetComponent<ChickenHealth>();

        if (chickenHealth) HandleChickenCollision(chickenHealth);
    }

    private void HandleChickenCollision(ChickenHealth chickenHealth)
    {
        // Impact Sound
        soundManager.PlayChickenHit();

        // Damage Poultry
        if (chickenHealth.health - damage <= 0)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.AddPlayerScore(chickenHealth.pointsReward);
            ShowPopup(chickenHealth.transform.position, $"{chickenHealth.pointsReward} {scorePopUpMsg}");
        }
        chickenHealth.TakeDamage(damage);

        // Canera Shake
        CameraShaker.instance.Shake(camShakeDuration, camShakeMagnitude);

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

    private void ShowPopup(Vector3 position, string msg)
    {
        // Point Indicator
        ScorePopup newPopUp = Instantiate(
            scorePopUp,
            position,
            Quaternion.identity
        ).GetComponent<ScorePopup>();
        newPopUp.SetText(msg);
        Destroy(newPopUp.gameObject, popupDestroyDelay);
    }
}
