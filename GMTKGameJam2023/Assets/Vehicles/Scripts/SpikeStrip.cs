using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeStrip : MonoBehaviour
{

    [SerializeField] private GameObject scorePopUp;

    [SerializeField] private int damage;

    [SerializeField] private float camShakeDuration;
    [SerializeField] private float camShakeMagnitude;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private float spikeHealth;
    private float spikeCurrentHealth;



    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        spikeCurrentHealth = spikeHealth;

    }

    private void TakeSpikeDamage(float damageTaken)
    {
        spikeHealth -= damageTaken;

        if (spikeHealth < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if Hit Chicken
        ChickenHealth chickenHealth = collision.gameObject.GetComponent<ChickenHealth>();
        if (chickenHealth == null && collision.transform.parent != null)
        {
            chickenHealth = collision.transform.parent.GetComponent<ChickenHealth>();
        }

        Car car = collision.gameObject.GetComponent<Car>();

        if (car == null)
        {
            car = collision.gameObject.GetComponent<Car>();
        }

        if (chickenHealth != null)
        {
            HandleChickenCollision(chickenHealth);
        }

        if (car != null)
        {
            HandleCarCollision(car);
        }
    }

    private void HandleCarCollision(Car car)
    {
        // Impact Sound
        // soundManager.PlayChickenHit();

        // Camera Shake


        
        

        if (car.canSpinOut == true && car.isSpinning == false && car.carName != "Police Car")
        {
            StartCoroutine(CameraShaker.instance.Shake(camShakeDuration, camShakeMagnitude));
            TakeSpikeDamage(5f);
            car.SpinOutCar();
        }
    }

    private void HandleChickenCollision(ChickenHealth chickenHealth)
    {
        // Impact Sound
        // soundManager.PlayChickenHit();

        // Camera Shake
        StartCoroutine(CameraShaker.instance.Shake(camShakeDuration, camShakeMagnitude));

        // Check if Chicken Will DIe
        if (chickenHealth.health - damage <= 0)
        {
            KillChicken(chickenHealth);
        }

        TakeSpikeDamage(1f);

        // Damage Poultry
        chickenHealth.TakeDamage(damage);
    }

    private void KillChicken(ChickenHealth chickenHealth)
    {
        // Increase Score
        gameManager.playerScore += chickenHealth.pointsReward;

        // Increase Kill Count
        gameManager.killCount++;

        // +100 Points Pop-Up
        ShowPopup(
            chickenHealth.transform.position,
            $"{chickenHealth.pointsReward * 1}"
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
        Destroy(newPopUp.gameObject, 0.7f);
    }
}
