using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    [SerializeField] private Car car;
    [SerializeField] private WreckerCart wreckerCart;
    protected GameManager gameManager;
    public GameObject hook;

    [Header("Camera Shake Values")]
    [SerializeField] private float camShakeDuration = 0.15f;
    [SerializeField] private float camShakeMagnitude = 0.25f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // lineRenderer = GetComponent<LineRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.isGameOver)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // Check if Hit Sheep
        Sheep sheep = collision.gameObject.GetComponent<Sheep>();
        if (sheep != null)
        {
            sheep.HandleDeath();
        }

        // Check if Hit Chicken
        ChickenHealth chickenHealth = collision.gameObject.GetComponent<ChickenHealth>();
        if (chickenHealth == null && collision.transform.parent != null)
            chickenHealth = collision.transform.parent.GetComponent<ChickenHealth>();

        BombChickenHealth bombChickenHealth = collision.gameObject.GetComponent<BombChickenHealth>();
        if (bombChickenHealth != null)
        {
            Debug.Log("Hit Bomb chicken");
        }

        if (chickenHealth != null)
        {
            car.HandleChickenCollision(chickenHealth);
            // rb.AddForce(transform.up * 10f);
            StartCoroutine(wreckerCart.HandleHitStop(chickenHealth.gameObject.GetComponent<ChickenMovement>().GetChickenHitstop()));
        }

        // Check if Hit Car
        Car hitCar = collision.gameObject.GetComponent<Car>();

        if (hitCar == null && collision.transform.parent != hitCar)
            hitCar = collision.transform.parent.GetComponent<Car>();

        if (hitCar != null)
        {
            wreckerCart.left = wreckerCart.left * -1;
            Debug.Log("We hit a car: " + collision.gameObject.name);
            HandleCarCollision(hitCar);
        }

        // Check if Hit Token
        WallController wall = collision.gameObject.GetComponent<WallController>();

        if(wall != null)
            HandleWallCollision(wall);
    }

    private void HandleCarCollision(Car car)
    {
        // TODO Impact Sound

        // TODO Camera Shake

        if (car.canSpinOut == true && car.isSpinning == false)
        {
            car.SpinOutCar(false);
            Debug.Log("car is spinning?");
        }
    }

    private void HandleWallCollision(WallController wall)
    {
        wall.WallHit();
        CameraShaker.instance.Shake(camShakeDuration, camShakeMagnitude);
        
        StartCoroutine(wreckerCart.HandleHitStop(0.01f));    
    }
}
