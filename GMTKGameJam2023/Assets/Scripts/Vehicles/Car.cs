using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Car : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject scorePopUp;

    [Header("Tags")]
    [SerializeField] private string objectBoundsTag = "Death Box";

    [Header("Speed")]
    [SerializeField] protected float carSpeed = 5f;

    [Header("PopUp Values")]
    [SerializeField] private string scorePopUpMsg = "Points";
    [SerializeField] private string tokenPopUpMsg = "Token";
    [SerializeField] private float popupDestroyDelay = 0.7f;

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
        TokenController token = collision.gameObject.GetComponent<TokenController>();

        if (chickenMovement != null)
        {
            // +100 Points Pop-Up
            ShowPopup(
                chickenMovement.transform.position, 
                $"{chickenMovement.pointsReward} {scorePopUpMsg}"
            );

            // Slaughter Poultry
            chickenMovement.KillChicken();

            // Canera Shake
            StartCoroutine(cameraShaker.Shake(camShakeDuration, camShakeMagnitude));

            // Increase Score
            gameManager.playerScore += chickenMovement.pointsReward;

            // Increase Kill Count
            gameManager.killCount++;
        }

        if (token != null)
        {
            ShowPopup(
                token.transform.position,
                $"{1} {tokenPopUpMsg}"
            );
            token.tokenCollected();
            gameManager.tokens++;
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
