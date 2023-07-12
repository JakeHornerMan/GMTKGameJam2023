using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Car : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject scorePopUp;
    [SerializeField] private string comboSymbol = "x";
    [SerializeField] private TextMeshProUGUI comboText;

    [Header("Car Info")]
    [SerializeField] public Sprite carSprite;
    [SerializeField] public string carName;
    [SerializeField] public int carPrice = 2;
    [SerializeField] private bool ignoreTokens = false;
    [SerializeField] private bool isSlicingCar = false;

    [Header("Tags")]
    [SerializeField] private string objectBoundsTag = "Death Box";

    [Header("Speed")]
    [SerializeField] protected float carSpeed = 5f;

    [Header("Particles")]
    [SerializeField] private ParticleSystem tokenCollectParticles;
    [SerializeField] private float particleDestroyDelay = 2f;

    [Header("PopUp Values")]
    [SerializeField] private string scorePopUpMsg = "Points";
    [SerializeField] private string tokenPopUpMsg = "Token";
    [SerializeField] private float popupDestroyDelay = 0.7f;

    [Header("Camera Shake Values")]
    [SerializeField] private float camShakeDuration = 0.1f;
    [SerializeField] private float camShakeMagnitude = 0.1f;

    private int carKillCount = 0;

    private GameManager gameManager;
    [HideInInspector] public CameraShaker cameraShaker;
    [HideInInspector] public SoundManager soundManager;

    private void Awake()
    {
        cameraShaker = FindObjectOfType<CameraShaker>();
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
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
        gameObject.TryGetComponent(out Rigidbody2D rb);

        if (rb != null)
            rb.velocity = transform.up * carSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if Hit Chicken
        ChickenMovement chickenMovement = collision.gameObject.GetComponent<ChickenMovement>();
        if (chickenMovement == null && collision.transform.parent != null)
            chickenMovement = collision.transform.parent.GetComponent<ChickenMovement>();

        // Check if Hit Token
        TokenController token = collision.gameObject.GetComponent<TokenController>();

        if (chickenMovement != null)
            HandleChickenCollision(chickenMovement);

        if (token != null & !ignoreTokens)
            HandleTokenCollision(token);

        if (collision.gameObject.CompareTag(objectBoundsTag))
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

    private void HandleChickenCollision(ChickenMovement chickenMovement)
    {
        // Increase Car-Specific Kill Count
        carKillCount++;

        // +100 Points Pop-Up
        ShowPopup(
            chickenMovement.transform.position,
            $"{chickenMovement.pointsReward * carKillCount} {scorePopUpMsg}"
        );

        // Slaughter Poultry
        chickenMovement.KillChicken();

        // Slice Sound
        if (isSlicingCar)
            soundManager.PlayRandomSlice();

        // Canera Shake
        StartCoroutine(cameraShaker.Shake(camShakeDuration, camShakeMagnitude));

        // Increase Score
        gameManager.playerScore += chickenMovement.pointsReward * carKillCount;

        // Increase Kill Count
        gameManager.killCount++;
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
