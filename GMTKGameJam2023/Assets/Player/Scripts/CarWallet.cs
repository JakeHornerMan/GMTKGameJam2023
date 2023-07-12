using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWallet : MonoBehaviour
{
    [Header("Wallet Values")]
    [SerializeField] private int walletLimit = 15;
    [SerializeField] private int startWalletCount = 10;
    [SerializeField] private float refillDelaySeconds = 2f;
    [SerializeField] private int amountPerRefill = 1;

    public int carCount = 0;

    private void Start()
    {
        carCount = startWalletCount;

        InvokeRepeating(nameof(RefillCars), refillDelaySeconds, refillDelaySeconds);
    }

    private void RefillCars()
    {
        carCount += amountPerRefill;
        carCount = Mathf.Clamp(carCount, 0, walletLimit);
    }
}
