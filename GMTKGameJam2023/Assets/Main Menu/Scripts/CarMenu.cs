using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class CarMenu : MonoBehaviour
{
    [Header("Car Info")]
    [SerializeField] public float carHealth = 100;     // Float for health
    [SerializeField] public bool canSpinOut = false;
    [SerializeField] public bool isSpinning = false;
    [SerializeField] private bool carInHitStop = false;
    private float degreesPerSecond = 540f;
    public bool carInAction = true; //is car in play (not being launched)?


    // [Header("Ultimate Info")]
    // [SerializeField] public bool isUltimate = false;
    // [SerializeField] public float ultimateResetTime;

    [Header("Tags")]
    [SerializeField] private string deathboxTag = "Death Box";

    [Header("Placement Criteria")]
    [SerializeField] public List<string> placeableLaneTags;
    [SerializeField] public bool placeableAnywhere = false;

    [Header("Speed")]
    [SerializeField] protected float carSpeed = 5f;
    private float currentSpeed;

    [Header("Damage")]
    [SerializeField] private int damage = 120;
    [SerializeField] private GameObject DeathExplosion;

    //[Header("References to Children")]
    private GameObject carSpriteObject;

    [SerializeField] private float carHitStopEffectMultiplier = 1.0f;

    [Header("Camera Shake Values")]
    [SerializeField] private float camShakeDuration = 0.15f;
    [SerializeField] private float camShakeMagnitude = 0.25f;

    [Header("Sound")]
    [SerializeField] private SoundConfig[] spawnSound;

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

    public virtual void Start()
    {
        SetCarSpeed(carSpeed);

        carSpriteObject = GetComponentInChildren<SpriteRenderer>().gameObject;

        StartCoroutine(DisableCarCollisionOnSpawn());

        //soundManager?.RandomPlaySound(spawnSound);

        // Shake Camera
        if (cameraShaker.isActiveAndEnabled)
            CameraShaker.instance.Shake(camShakeDuration, camShakeMagnitude);
    }

    private void Update()
    {
        if (isSpinning)
        {
            carSpriteObject.transform.Rotate(new Vector3(0, 0, degreesPerSecond) * Time.deltaTime);
        }
    }

    protected virtual void SetCarSpeed(float speed)
    {
        if (rb != null)
            rb.velocity = transform.up * speed;
            currentSpeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if Hit Chicken
        ChickenMenu chicken = collision.gameObject.GetComponent<ChickenMenu>();
        if (chicken == null && collision.transform.parent != null)
            chicken = collision.transform.parent.GetComponent<ChickenMenu>();

        // Check if Hit Wall
        BarrierMenu barrier = collision.gameObject.GetComponent<BarrierMenu>();

        if (chicken != null)
            HandleChickenCollision(chicken);

        if(barrier != null)
            HandleBarrierCollision(barrier);
    }


    private void HandleBarrierCollision(BarrierMenu barrier)
    {
        LaunchCar();
        barrier.BarrierHit();

        CameraShaker.instance.Shake(camShakeDuration, camShakeMagnitude);
        StartCoroutine(CarHitStop(0.1f));
    }

    public virtual void HandleChickenCollision(ChickenMenu chicken)
    {

        // Hit Stop, TurboChicken has different movement script;
        StartCoroutine(CarHitStop(chicken.gameObject.GetComponent<ChickenMenu>().GetChickenHitstop()));

        // Damage Poultry
        chicken.TakeDamage(damage);

        // Canera Shake
        CameraShaker.instance.Shake(camShakeDuration, camShakeMagnitude);

        // Impact Sound
        soundManager.PlayChickenHit();
    }

    public void SpinOutCar(bool launching)
    {
        if (canSpinOut == true && isSpinning == false)
        {
            if (!launching)
            {
                BoxCollider2D carCollider = GetComponent<BoxCollider2D>();
                carCollider.size = new Vector2(1.8f, carCollider.size.y);
            }            

            isSpinning = true;
        }
    }

    private IEnumerator CarHitStop(float hitStopLength)
    {
        if (rb != null && carInAction == true && carInHitStop == false)
        {
            carInHitStop = true;

            // Store the original speed before entering the hit stop
            float originalSpeed = rb.velocity.y;

            rb.velocity = Vector3.zero;

            yield return new WaitForSecondsRealtime(hitStopLength * carHitStopEffectMultiplier);

            if (carInAction == false)
            {
                yield break;
            }

            // Restore the original speed after the hit stop
            rb.velocity = transform.up * currentSpeed;

            carInHitStop = false;
        }
    }

    public void LaunchCar()
    {
        carInAction = false;

        // Generate random x and y components between -10 and 10
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomY = UnityEngine.Random.Range(-1f, 1f);

        // Round to -0.5 or 0.5 if the number is between -0.5 and 0.5
        if (randomX > -0.5f && randomX < 0.5f)
        {
            randomX = (randomX > 0) ? 0.5f : -0.5f;
        }

        if (randomY > -0.5f && randomY < 0.5f)
        {
            randomY = (randomY > 0) ? 0.5f : -0.5f;
        }

        // Create a Vector2 with the random components
        Vector2 randomVector = new Vector2(randomX, randomY);

        // Normalize the Vector2
        Vector2 normalizedVector = randomVector.normalized;

        Collider2D collider = gameObject.GetComponent<Collider2D>();

        if (collider != null){
            GetComponent<Collider2D>().enabled = false;
        }else{
            GetComponentInChildren<Collider2D>().enabled = false;
        }

        rb.velocity = normalizedVector * 20;

        GameObject currentBomb = Instantiate(DeathExplosion, transform.position, Quaternion.identity);

        Destroy(currentBomb, 0.85f);

        float carLaunchScale = 12;

        StartCoroutine(LaunchCarCoroutine(new Vector3(carLaunchScale, carLaunchScale, 1), new Vector3(normalizedVector.x, normalizedVector.y, gameObject.transform.position.z)));

    }

    private IEnumerator LaunchCarCoroutine(Vector3 targetScale, Vector3 targetPos)
    {
        Vector3 initialScale = gameObject.transform.localScale;
        Vector3 initialPos = gameObject.transform.position;
        float t = 0;

        float launchSpeed = 0.5f;

        canSpinOut = true;
        SpinOutCar(true);

        while (t < 1.5)
        {
            t += Time.deltaTime * launchSpeed;

            // Linearly interpolate vehicle scale
            gameObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }
    }

    private IEnumerator DisableCarCollisionOnSpawn()
    {
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        GetComponent<Collider2D>().enabled = true;
    }

    public virtual void CarGoesOffscreen()
    {
        DestroySelf();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
