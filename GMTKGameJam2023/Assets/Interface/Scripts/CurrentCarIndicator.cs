using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentCarIndicator : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] public bool followCursor = false;
    [SerializeField] private Vector2 cornerPosition;

    [Header("References")]
    [SerializeField] private SpriteRenderer carSprite;
    [SerializeField] private TextMeshProUGUI carName;
    [SerializeField] private TextMeshProUGUI carCost;
    [SerializeField] private TextMeshProUGUI totalTokens;
    [SerializeField] private Image walletRefillTimer;
    [SerializeField] private TextMeshProUGUI carWalletCountText;

    [Header("UI Display")]
    [SerializeField] private string tokenLabel = "Token";
    [Tooltip("Text to show after token cost, e.g. 2 {Token}")]

    [SerializeField] private string totalTokenLabel = "Total";
    [Tooltip("Text to show after total tokens, e.g. 6 {Total}")]

    [Header("Colors")]
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;

    private CarWallet carWallet;

    private void Awake()
    {
        carWallet = FindObjectOfType<CarWallet>();
    }

    private void Start()
    {
        if (!followCursor)
            transform.position = cornerPosition;
    }

    public void SetUI(Car currentActiveCar, int playerCash, int carWalletCount)
    {
        carSprite.sprite = currentActiveCar.GetComponent<ObjectInfo>().objectSprite;
        carName.text = currentActiveCar.GetComponent<ObjectInfo>().objectName;
        carCost.text = $"{currentActiveCar.carPrice} {tokenLabel}";
        totalTokens.text = $"{playerCash} {totalTokenLabel}";
        carCost.color = playerCash >= currentActiveCar.carPrice ? positiveColor : negativeColor;
        carWalletCountText.text = carWalletCount.ToString();
        walletRefillTimer.fillAmount = 1 - (carWallet.timeUntilRefill / carWallet.refillDelaySeconds);
    }
}
