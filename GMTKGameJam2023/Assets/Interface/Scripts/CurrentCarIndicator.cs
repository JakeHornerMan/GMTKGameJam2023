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
    [SerializeField] private GameObject tokenIcon;
    [SerializeField] private Image walletRefillTimer;
    [SerializeField] private GameObject walletRefillTimerBG;
    [SerializeField] private GameObject walletRefillTimerCircle;
    [SerializeField] private TextMeshProUGUI carWalletCountText;

    [Tooltip("Newly added one-line token display, {cost}/{have}")]
    [SerializeField] private TextMeshProUGUI combinedTokenDisplay;

    [Header("UI Display")]
    [SerializeField] private string tokenLabel = "Token";
    [Tooltip("Text to show after token cost, e.g. 2 {Token}")]

    [SerializeField] private string totalTokenLabel = "Total";
    [Tooltip("Text to show after total tokens, e.g. 6 {Total}")]

    [Header("Colors")]
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color positiveBlueColor;
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
        // Enable Things unrelated to ultimate
        walletRefillTimer.gameObject.SetActive(true);
        carWalletCountText.gameObject.SetActive(true);
        walletRefillTimerBG.gameObject.SetActive(true);
        tokenIcon.gameObject.SetActive(true);
        walletRefillTimerCircle.SetActive(true);

        carSprite.sprite = currentActiveCar.GetComponent<ObjectInfo>().objectSprite;
        carName.text = currentActiveCar.GetComponent<ObjectInfo>().objectName;

        // NOT BEING USED, DEACTIVATED IN GAME UI
        carCost.text = $"{currentActiveCar.carPrice} {tokenLabel}";
        totalTokens.text = $"{playerCash} {totalTokenLabel}";
        carCost.color = playerCash >= currentActiveCar.carPrice ? positiveColor : negativeColor;

        carWalletCountText.gameObject.SetActive(true);
        carWalletCountText.text = carWalletCount.ToString();
        walletRefillTimer.gameObject.SetActive(true);
        walletRefillTimer.fillAmount = 1 - (carWallet.timeUntilRefill / carWallet.refillDelaySeconds);

        // New single-line token display
        combinedTokenDisplay.text = $"{currentActiveCar.carPrice}/{playerCash}";
        combinedTokenDisplay.color = playerCash >= currentActiveCar.carPrice ? positiveBlueColor : negativeColor;
    }

    public void SetUI(Ultimate currentUlt)
    {
        // Disable Things unrelated to ultimate
        walletRefillTimer.gameObject.SetActive(false);
        carWalletCountText.gameObject.SetActive(false);
        walletRefillTimerBG.gameObject.SetActive(false);
        tokenIcon.gameObject.SetActive(false);
        walletRefillTimerCircle.SetActive(false);

        carSprite.sprite = currentUlt.GetComponent<ObjectInfo>().objectSprite;
        carName.text = currentUlt.GetComponent<ObjectInfo>().objectName;
        combinedTokenDisplay.text = string.Empty;
    }
}
