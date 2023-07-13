using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentCarIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer carSprite;
    [SerializeField] private TextMeshProUGUI carName;
    [SerializeField] private TextMeshProUGUI carCost;
    [SerializeField] private TextMeshProUGUI totalTokens;
    [SerializeField] private Image walletRefillTimer;
    [SerializeField] private TextMeshProUGUI carWalletCountText;

    [Header("UI Display")]
    [SerializeField] private string tokenLabel = "Token";
    [SerializeField] private string totalTokenLabel = "Total";

    [Header("Colors")]
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;

    public void SetUI(Car currentActiveCar, int playerCash, int carWalletCount)
    {
        carSprite.sprite = currentActiveCar.carSprite;
        carName.text = currentActiveCar.carName;
        carCost.text = $"{currentActiveCar.carPrice} {tokenLabel}";
        totalTokens.text = $"{playerCash} {totalTokenLabel}";
        carCost.color = playerCash >= currentActiveCar.carPrice ? positiveColor : negativeColor;
        carWalletCountText.text = carWalletCount.ToString();
    }
}
