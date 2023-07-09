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
    public CameraShaker cameraShaker;

    private void Awake()
    {
        cameraShaker = FindObjectOfType<CameraShaker>();
        gameManager = FindObjectOfType<GameManager>();
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
        ChickenMovement chickenMovement = collision.gameObject.GetComponent<ChickenMovement>();

        // If there's no ChickenMovement script on the game object, check the parent
        if (chickenMovement == null && collision.transform.parent != null)
        {
            chickenMovement = collision.transform.parent.GetComponent<ChickenMovement>();
        }

        TokenController token = collision.gameObject.GetComponent<TokenController>();

        if (chickenMovement != null)
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

            // Canera Shake
            StartCoroutine(cameraShaker.Shake(camShakeDuration, camShakeMagnitude));

            // Increase Score
            gameManager.playerScore += chickenMovement.pointsReward * carKillCount;

            // Increase Kill Count
            gameManager.killCount++;
        }

        if (token != null)
        {
            GameObject newTokenParticles = Instantiate(
                tokenCollectParticles.gameObject,
                token.transform.position,
                Quaternion.identity
            );
            Destroy(newTokenParticles, particleDestroyDelay);

            ShowPopup(
                token.transform.position,
                $"{1} {tokenPopUpMsg}"
            );

            token.TokenCollected();
            gameManager.tokens++;
            gameManager.totalTokens++;
        }

        if (collision.gameObject.CompareTag(objectBoundsTag))
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
