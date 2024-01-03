using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using TMPro;

public class Ultimate : MonoBehaviour
{
    [Header("Ultimate Info")]
    [SerializeField] private bool canIBeBombed = true;

    [Header("References")]
    [SerializeField] private GameObject scorePopUp;
    [SerializeField] private string comboSymbol = "x";
    [Tooltip("Synbol used before combo count, e.g. {x}4")]
    [SerializeField] private TextMeshProUGUI comboText;
    protected GameManager gameManager;
    private Rigidbody2D rb;
    
    [Header("Damage")]
    [SerializeField] private int damage = 120;
    [SerializeField] private float comboMultiplier = 0.2f;
    private float defaultComboMultiplier = 1f;
    [SerializeField] private GameObject DeathExplosion;

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
    [SerializeField] private float carHitStopEffectMultiplier = 1.0f;

    [Header("Camera Shake Values")]
    [SerializeField] private float camShakeDuration = 0.15f;
    [SerializeField] private float camShakeMagnitude = 0.05f;

    [Header("Sound")]
    [SerializeField] private SoundConfig[] spawnSound;

    private int carKillCount = 0;
    protected int totalPoints = 0;

    [HideInInspector] public CameraShaker cameraShaker;
    [HideInInspector] public SoundManager soundManager;

    private void Awake()
    {
        cameraShaker = FindObjectOfType<CameraShaker>();
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        // Check if Chicken Will Die
        if (chickenHealth.health - damage <= 0)
        {
            KillChicken(chickenHealth);
        }
        // StartCoroutine(CarHitStop(chickenHealth.gameObject.GetComponent<ChickenMovement>().GetChickenHitstop()));

        // Damage Poultry
        chickenHealth.TakeDamage(damage);

        // Destroy Self if Bomb Chicken
        BombChickenHealth bombChickenHealth = chickenHealth as BombChickenHealth;
        if (bombChickenHealth != null && canIBeBombed)
        {
            // Destroy the car as well
            Destroy(gameObject);
        }

        // Canera Shake
        StartCoroutine(cameraShaker.Shake(camShakeDuration, camShakeMagnitude));

        // Impact Sound
        soundManager.PlayChickenHit();
    }

    protected virtual void HandleSheepCollision(Sheep sheep)
    {
        sheep.HandleDeath();
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

    public void DestroySelf()
    {
        if (totalPoints > 0)
            gameManager.AddPlayerScore(totalPoints);
        Destroy(gameObject);
    }
}
