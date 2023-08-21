using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashChicken : ChickenHealth
{
    [SerializeField] public bool dropTheBag = false;
    [SerializeField] public int tokenAmount = 5;
    [SerializeField] private GameObject tokenBag;
    [SerializeField] private GameObject scorePopUp;
    [SerializeField] private string tokenPopUpMsg = "Token";
    [SerializeField] private float popupDestroyDelay = 0.7f;

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

        if(dropTheBag){
            Instantiate(tokenBag, transform.position, Quaternion.identity);
        }
        else{        
            gameManager.AddTokens(tokenAmount);
            soundManager.PlayCashChicken();
        }
        
        Destroy(gameObject);
    }
}
