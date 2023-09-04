using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashChicken : ChickenHealth
{
    [Header("Cash Chicken Values")]
    [SerializeField] public bool dropTheBag = false;
    [SerializeField] public int tokenAmount = 5;

    [Header("References")]
    [SerializeField] private GameObject tokenBag;

    private GameManager gameManager;
    private SoundManager soundManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    protected override void HandleDeath()
    {
        Vector3 particlePos = new(transform.position.x, transform.position.y, featherParticlesZPos);
        Instantiate(featherParticles, particlePos, Quaternion.identity);

        if (dropTheBag) Instantiate(tokenBag, transform.position, Quaternion.identity);
        else gameManager.AddTokens(tokenAmount);

        soundManager.PlayCashChicken();

        Destroy(gameObject);
    }
}
