using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Car : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject scorePopUp;
    [SerializeField] private string comboSymbol = "x";
    [Tooltip("Synbol used before combo count, e.g. {x}4")]
    [SerializeField] private TextMeshProUGUI comboText;

    [Header("Car Info")]
    [SerializeField] public Sprite carSprite;
    [SerializeField] public string carName;
    [SerializeField] public int carPrice = 2;
    [SerializeField] private bool ignoreTokens = false;
    [SerializeField] private bool isSlicingCar = false;

    [Header("Tags")]
    [SerializeField] private string deathboxTag = "Death Box";

    [Header("Speed")]
    [SerializeField] protected float carSpeed = 5f;

    [Header("Damage")]
    [SerializeField] private int damage = 120;
    [SerializeField] private float comboPointsMultiplier = 0.2f;

    [Header("Particles")]
    [SerializeField] private ParticleSystem tokenCollectParticles;
    [SerializeField] private float particleDestroyDelay = 2f;

    [Header("PopUp Values")]
    [SerializeField] private string scorePopUpMsg = "Points";
    [Tooltip("Text after getting points, e.g. 100 {Poitns}")]
    [SerializeField] private string tokenPopUpMsg = "Token";
    [Tooltip("Text after getting tokens, e.g. 1 {Token}")]
    [SerializeField] private float popupDestroyDelay = 0.7f;

    [Header("Camera Shake Values")]
    [SerializeField] private float camShakeDuration = 0.1f;
    [SerializeField] private float camShakeMagnitude = 0.1f;

    private int carKillCount = 0;

    private GameManager gameManager;
    private Rigidbody2D rb;
    [HideInInspector] public CameraShaker cameraShaker;
    [HideInInspector] public SoundManager soundManager;

    private void Awake()
    {
        cameraShaker = FindObjectOfType<CameraShaker>();
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        carKillCount = 0;
    }

    private void Update()
    {
        // xKillCount Combo Text
        comboText.text = $"{comboSymbol}{carKillCount}";
    }

    protected virtual void SetCarSpeed()
    {
        if (rb != null)
            rb.velocity = transform.up * carSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.gameOver)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // Check if Hit Chicken
        ChickenHealth chickenHealth = collision.gameObject.GetComponent<ChickenHealth>();
        if (chickenHealth == null && collision.transform.parent != null)
            chickenHealth = collision.transform.parent.GetComponent<ChickenHealth>();

        // Check if Hit Token
        TokenController token = collision.gameObject.GetComponent<TokenController>();

        if (chickenHealth != null)
            HandleChickenCollision(chickenHealth);

        if (token != null & !ignoreTokens)
            HandleTokenCollision(token);

        if (collision.gameObject.CompareTag(deathboxTag))
            Destroy(gameObject);
    }

    private void HandleTokenCollision(TokenController token)
    {
        // Token Particles
        GameObject newTokenParticles = Instantiate(
                        tokenCollectParticles.gameObject,
                        token.transform.position,
                        Quaternion.identity
                    );
        Destroy(newTokenParticles, particleDestroyDelay);

        // +1 Token Popup
        ShowPopup(
            token.transform.position,
            $"{1} {tokenPopUpMsg}"
        );

        // Collect Tokens
        token.TokenCollected();
        gameManager.tokens++;
        gameManager.totalTokens++;
    }

    private void HandleChickenCollision(ChickenHealth chickenHealth)
    {
        // Impact Sound
        soundManager.PlayChickenHit();

        // Slice Sound
        if (isSlicingCar)
            soundManager.PlayRandomSlice();

        // Canera Shake
        StartCoroutine(cameraShaker.Shake(camShakeDuration, camShakeMagnitude));

        // Check if Chicken Will DIe
        if (chickenHealth.health - damage <= 0)
        {
            KillChicken(chickenHealth);
        }

        // Damage Poultry
        chickenHealth.TakeDamage(damage);
    }

    private void KillChicken(ChickenHealth chickenHealth)
    {
        // Increase Score
        gameManager.playerScore += chickenHealth.pointsReward * carKillCount;

        // Increase Kill Count
        gameManager.killCount++;

        // Increase Car-Specific Kill Count
        carKillCount++;

        // +100 Points Pop-Up
        ShowPopup(
            chickenHealth.transform.position,
            $"{chickenHealth.pointsReward * carKillCount} {scorePopUpMsg}"
        );
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
