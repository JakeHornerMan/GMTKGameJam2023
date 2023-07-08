using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Car : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string objectBoundsTag = "Death Box";

    [Header("Speed")]
    [SerializeField] protected float carSpeed = 5f;

    [Header("Camera Shake Values")]
    [SerializeField] private float camShakeDuration = 0.1f;
    [SerializeField] private float camShakeMagnitude = 0.1f;

    private GameManager gameManager;
    private CameraShaker cameraShaker;

    private void Awake()
    {
        cameraShaker = FindObjectOfType<CameraShaker>();
        gameManager = FindObjectOfType<GameManager>();
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
        if (chickenMovement != null)
        {
            chickenMovement.KillChicken();
                        
            // Canera Shake
            StartCoroutine(cameraShaker.Shake(camShakeDuration, camShakeMagnitude));

            // Increase Score
            gameManager.playerScore += chickenMovement.pointsReward;

            // Increase Kill Count
            gameManager.killCount++;
        }

        if (collision.gameObject.CompareTag(objectBoundsTag))
        {
            Debug.Log("CAR HIT END");
            Destroy(gameObject);
        }
    }
}
