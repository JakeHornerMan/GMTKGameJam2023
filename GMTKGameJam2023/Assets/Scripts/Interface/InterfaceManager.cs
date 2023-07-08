using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InterfaceManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI carWalletCountText;

    private GameManager gameManager;
    private CarWallet carWallet;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        carWallet = FindObjectOfType<CarWallet>();
    }

    private void Update()
    {
        pointsText.text = gameManager.playerScore.ToString("0000");
        killsText.text = gameManager.killCount.ToString("000");
        carWalletCountText.text = carWallet.carCount.ToString("00");
    }
}
