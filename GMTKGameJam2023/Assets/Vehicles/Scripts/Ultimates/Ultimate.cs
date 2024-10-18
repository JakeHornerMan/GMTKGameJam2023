using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using TMPro;

public class Ultimate : MonoBehaviour
{
    [Header("Ultimate Info")]
    [SerializeField] private bool canIBeBombed = true;
    [SerializeField] public float ultimateResetTime;
    [SerializeField] public bool placeableAnywhere = true;
    [SerializeField] public bool ignoreTokens = true;

    [Header("Buy Info")]
    [SerializeField] public int ultimateShopPrice;

    [Header("References")]
    [SerializeField] public GameObject scorePopUp;
    [SerializeField] private string comboSymbol = "x";
    [Tooltip("Synbol used before combo count, e.g. {x}4")]
    [SerializeField] private TextMeshProUGUI comboText;
    protected GameManager gameManager;
    protected TutorialManager tutorialManager;
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
    [SerializeField] public SoundConfig[] spawnSound;

    private int carKillCount = 0;
    protected int totalPoints = 0;

    [HideInInspector] public CameraShaker cameraShaker;
    public SoundManager soundManager;

    private void Awake()
    {
        cameraShaker = FindObjectOfType<CameraShaker>();
        gameManager = FindObjectOfType<GameManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        soundManager = FindObjectOfType<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
        soundManager?.RandomPlaySound(spawnSound);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameManager != null){
            if (gameManager.isGameOver)
            {
                rb.velocity = Vector2.zero;
                return;
            }
        }

        // Debug.Log(collision.gameObject.name);

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

        Car car = collision.gameObject.GetComponent<Car>();

        if (car != null)
            car.LaunchCar();

        
        WallController wall = collision.gameObject.GetComponent<WallController>();
        wall?.WallHit();
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

        if(gameManager != null) gameManager.UpdateTokens(1);
        if(tutorialManager != null)tutorialManager.UpdateTokens(1);

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
        // BombChickenHealth bombChickenHealth = chickenHealth as BombChickenHealth;
        // if (bombChickenHealth != null && canIBeBombed)
        // {
        //     // Destroy the car as well
        //     Destroy(gameObject);
        // }

        // Canera Shake
        CameraShaker.instance.Shake(camShakeDuration, camShakeMagnitude);

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
        if(gameManager != null) gameManager.killCount++;
        if(tutorialManager != null) tutorialManager.killCount++;

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

        if (comboText != null)
        {
            // Debug.Log(comboSymbol + carKillCount);
            comboText.text = comboSymbol + carKillCount;
            Debug.Log(comboText.text);
        }
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

    public void OnDestroy()
    {
        if (totalPoints > 0){
            if(gameManager != null) gameManager.AddPlayerScore(totalPoints);
            if(tutorialManager != null) tutorialManager.AddPlayerScore(totalPoints);
        }
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
