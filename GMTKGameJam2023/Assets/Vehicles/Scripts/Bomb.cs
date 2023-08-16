using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject scorePopUp;

    [SerializeField] private float bombLifetime;

    [Header("Damage")]
    [SerializeField] private int damage = 120;
    [SerializeField] private float comboMultiplier = 0.2f;
    private float defaultComboMultiplier = 1f;

    private int carKillCount = 0;
    private int totalPoints = 0;

    [Header("Camera Shake Values")]
    [SerializeField] private float camShakeDuration = 0.1f;
    [SerializeField] private float camShakeMagnitude = 0.1f;

    private GameManager gameManager;
    [HideInInspector] public SoundManager soundManager;

    private BombingJet jetParent;

    private void Start()
    {

        //jetParent = transform.parent.GetComponent<BombingJet>();

        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();

        StartCoroutine(BombScoreManager());

        
    }

    public void AssignJetParent(BombingJet jet)
    {
        jetParent = jet;
    }

    private IEnumerator BombScoreManager()
    {

        yield return new WaitForSeconds(bombLifetime);

        gameObject.SetActive(false);

        jetParent.AddBombScore(totalPoints);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (gameManager.isGameOver)
        //{
        //    rb.velocity = Vector2.zero;
        //    return;
        //}

        // Check if Hit Chicken
        ChickenHealth chickenHealth = collision.gameObject.GetComponent<ChickenHealth>();
        if (chickenHealth == null && collision.transform.parent != null)
            chickenHealth = collision.transform.parent.GetComponent<ChickenHealth>();

        if (chickenHealth != null)
            HandleChickenCollision(chickenHealth);
    }

    private void HandleChickenCollision(ChickenHealth chickenHealth)
    {
        // Impact Sound
        soundManager.PlayChickenHit();

        

        // Camera Shake
        StartCoroutine(CameraShaker.instance.Shake(camShakeDuration, camShakeMagnitude));

        // Check if Chicken Will DIe
        if (chickenHealth.health - damage <= 0)
        {
            KillChicken(chickenHealth);
        }

        //chickenHealth.gameObject.GetComponent<ChickenMovement>().PlayChickenHitstop();

        //StartCoroutine(CarHitStop(chickenHealth.gameObject.GetComponent<ChickenMovement>().GetChickenHitstop()));

        // Damage Poultry
        chickenHealth.TakeDamage(damage);
    }

    private void KillChicken(ChickenHealth chickenHealth)
    {
        // Increase Kill Count
        gameManager.killCount++;

        // Increase Car-Specific Kill Count
        carKillCount++;

        // Increase Score
        totalPoints = totalPoints + (chickenHealth.pointsReward * carKillCount);
        // gameManager.AddPlayerScore(chickenHealth.pointsReward * carKillCount);

        // Change Combo Multiplier
        float currentComboMultiplier = defaultComboMultiplier + (comboMultiplier * (carKillCount - 1));



        // +100 Points Pop-Up
        ShowPopup(
            chickenHealth.transform.position,
            $"{chickenHealth.pointsReward * currentComboMultiplier}"
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
