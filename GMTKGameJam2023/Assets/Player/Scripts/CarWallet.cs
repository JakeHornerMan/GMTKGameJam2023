using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWallet : MonoBehaviour
{
    [Header("Wallet Values")]
    [SerializeField] public int walletLimit = 10;
    [SerializeField] private int startWalletCount = 10;

    private bool walletEnabled = true;

    [SerializeField] public float refillDelaySeconds = 2f;
    [HideInInspector] public float timeUntilRefill = 0f;

    [SerializeField] private int amountPerRefill = 1;

    [HideInInspector] public int carCount = 0;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        if(!gameManager.devMode){
            walletLimit = PlayerValues.carWalletNodes;
            carCount = PlayerValues.carWalletNodes;
        }
        else{
            carCount = startWalletCount;
        }
        timeUntilRefill = refillDelaySeconds;

        walletEnabled = true;

        // start the refill coroutine
        StartCoroutine(RefillCars());
    }

    private IEnumerator RefillCars()
    {
        // infinite loop, be careful with these!
        while (walletEnabled && carCount < walletLimit)
        {
            // wait for the refill delay
            yield return new WaitForSeconds(refillDelaySeconds);
            carCount += amountPerRefill;
            carCount = Mathf.Clamp(carCount, 0, walletLimit);
            // reset timeUntilRefill
            timeUntilRefill = refillDelaySeconds;
        }

        walletEnabled = false;
    }

    private void Update()
    {
        // Reduce timeUntilRefill by the time passed since last frame
        if (timeUntilRefill > 0 && carCount < walletLimit)
        {
            timeUntilRefill -= Time.deltaTime;
        }

        if (walletEnabled == false)
        {
            if (carCount < walletLimit)
            {
                walletEnabled = true;
                // start the refill coroutine
                StartCoroutine(RefillCars());
            }
        }
    }
}
