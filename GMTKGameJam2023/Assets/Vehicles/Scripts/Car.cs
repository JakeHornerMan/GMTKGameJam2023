using System;
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
    [SerializeField] public Sprite carIcon;
    [SerializeField] public string carName;
    [SerializeField] public int carPrice = 2;
    [SerializeField] private bool ignoreTokens = false;
    [SerializeField] private bool isSlicingCar = false;
    [SerializeField] private bool canIBeBombed = true;
    [SerializeField] public bool canSpinOut = false;
    [SerializeField] public bool isSpinning = false;
    private float degreesPerSecond = 540f;

    [Header("Tags")]
    [SerializeField] private string deathboxTag = "Death Box";

    [Header("Placement Criteria")]
    [SerializeField] public List<string> placeableLaneTags;
    [SerializeField] public bool placeableAnywhere = false;

    [Header("Speed")]
    [SerializeField] protected float carSpeed = 5f;

    [Header("Damage")]
    [SerializeField] private int damage = 120;
    [SerializeField] private float comboMultiplier = 0.2f;
    private float defaultComboMultiplier = 1f;

    [Header("Particles")]
    [SerializeField] private ParticleSystem tokenCollectParticles;
    [SerializeField] private float particleDestroyDelay = 2f;

    //[Header("References to Children")]
    private GameObject carSpriteObject;

    [Header("PopUp Values")]
    [SerializeField] private string scorePopUpMsg = "";
    [Tooltip("Text after getting points, e.g. 100 {Poitns}")]
    [SerializeField] private string tokenPopUpMsg = "Token";
    [Tooltip("Text after getting tokens, e.g. 1 {Token}")]
    [SerializeField] private float popupDestroyDelay = 0.7f;

    [Header("Camera Shake Values")]
    [SerializeField] private float camShakeDuration = 0.15f;
    [SerializeField] private float camShakeMagnitude = 0.05f;

    private int carKillCount = 0;
    protected int totalPoints = 0;

    [SerializeField] private float carHitStopEffectMultiplier = 1.0f;

    protected GameManager gameManager;
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

        if (isSpinning)
        {
            carSpriteObject.transform.Rotate(new Vector3(0, 0, degreesPerSecond) * Time.deltaTime);
        }

        // should be moved to function
        if (comboText != null)
        {
            comboText.text = $"{comboSymbol}{carKillCount}";
        }

    }

    protected virtual void SetCarSpeed()
    {
        if (rb != null)
            rb.velocity = transform.up * carSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.isGameOver)
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
        {
            //if (totalPoints > 0)
            //    gameManager.AddPlayerScore(totalPoints);
            //Destroy(gameObject);
        }
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
            $"{token.tokenValue} {tokenPopUpMsg}"
        );

        // Collect Tokens
        token.TokenCollected();

        gameManager.UpdateTokens(1);
        
    }

    public virtual void HandleChickenCollision(ChickenHealth chickenHealth)
    {
        // Impact Sound
        soundManager.PlayChickenHit();

        // Slice Sound
        if (isSlicingCar)
            soundManager.PlayRandomSlice();

        // Canera Shake
        StartCoroutine(cameraShaker.Shake(camShakeDuration, camShakeMagnitude));

        // Check if Chicken Will Die
        if (chickenHealth.health - damage <= 0)
        {
            KillChicken(chickenHealth);
        }

        //chickenHealth.gameObject.GetComponent<ChickenMovement>().PlayChickenHitstop();

        StartCoroutine(CarHitStop(chickenHealth.gameObject.GetComponent<ChickenMovement>().GetChickenHitstop()));

        // Damage Poultry
        chickenHealth.TakeDamage(damage);

        // Destroy Self if Bomb Chicken
        BombChickenHealth bombChickenHealth = chickenHealth as BombChickenHealth;
        if (bombChickenHealth != null && canIBeBombed)
        {
            // Destroy the car as well
            Destroy(gameObject);
        }
    }

    private void KillChicken(ChickenHealth chickenHealth)
    {
        // Increase Kill Count
        gameManager.killCount++;

        // Increase Car-Specific Kill Count
        carKillCount++;

        // Increase Score
        totalPoints += chickenHealth.pointsReward * carKillCount;
        // gameManager.AddPlayerScore(chickenHealth.pointsReward * carKillCount);

        // Change Combo Multiplier
        float currentComboMultiplier = defaultComboMultiplier + (comboMultiplier * (carKillCount - 1));

        // +100 Points Pop-Up
        ShowPopup(
            chickenHealth.transform.position,
            $"{chickenHealth.pointsReward * currentComboMultiplier} {scorePopUpMsg}"
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

    public void SpinOutCar()
    {
        if (canSpinOut == true && isSpinning == false)
        {
            BoxCollider2D carCollider = GetComponent<BoxCollider2D>();

            carCollider.size = new Vector2(1.8f, carCollider.size.y);

            carSpriteObject = GetComponentInChildren<SpriteRenderer>().gameObject;

            isSpinning = true;
        }
    }

    private IEnumerator CarHitStop(float hitStopLength)
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;

            yield return new WaitForSecondsRealtime(hitStopLength * carHitStopEffectMultiplier);

            rb.velocity = transform.up * carSpeed;
        }
        
    }

    public virtual void CarGoesOffscreen()
    {
        if (totalPoints > 0)
            gameManager.AddPlayerScore(totalPoints);
        Destroy(gameObject);
    }

    public void DestroySelf()
    {
        if (totalPoints > 0)
            gameManager.AddPlayerScore(totalPoints);
        Destroy(gameObject);
    }
}
