using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    [SerializeField] public Car car;
    [SerializeField] public WreckerCart wreckerCart;
    protected GameManager gameManager;
    public GameObject hook;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        RenderRope();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.isGameOver)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // Debug.Log(collision.gameObject.name);

        // Check if Hit Chicken
        ChickenHealth chickenHealth = collision.gameObject.GetComponent<ChickenHealth>();
        if (chickenHealth == null && collision.transform.parent != null)
            chickenHealth = collision.transform.parent.GetComponent<ChickenHealth>();

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
    }

    private void HandleCarCollision(Car car)
    {
        // Impact Sound

        // Camera Shake


        if (car.canSpinOut == true && car.isSpinning == false)
        {
            car.SpinOutCar();
            Debug.Log("car is spinning?");
        }
    }

    private void RenderRope()
    {
        Vector3[] positions = { this.transform.position, hook.transform.position };
        lineRenderer.SetPositions(positions);
    }
}
