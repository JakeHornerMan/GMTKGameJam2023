using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{

    [Header("Animation")]
    [SerializeField] private string speedUpTextFadeOutTrigger = "FadeOut";

    [Header("Timing")]
    [SerializeField] private float speedUpTextDuration = 1.3f;
    [SerializeField] private float speedUpTextDeactivationDelay = 2;

    [Header("UI References")]
    [SerializeField] private string canvas_Name = "Canvas";
    [SerializeField] private string pointsText_Name = "ScoreText";
    [SerializeField] private string killsText_Name = "KillCount";
    [SerializeField] private string carWalletCount_Name = "WalletCount";
    [SerializeField] private string carWalletRadialUI_Name = "WalletRadial";
    [SerializeField] private string tokensText_Name = "TokenCount";
    [SerializeField] private string timeText_Name = "TimeText";
    [SerializeField] private string currentCarText_Name = "SelectedCarNameText";
    [SerializeField] private string speedUpText_Name = "SpeedUpText";
    [SerializeField] private string missedChickenCount_Name = "MissCount";

    private int scoreForText;
    private int scoreMoverPositive;
    private int scoreMoverNegative;

    private GameManager gameManager;
    private VehicleSpawner vehicleSpawner;
    private CarWallet carWallet;
    private Animator speedUptextAnimator;

    // UI References
    private GameObject canvas;
    private GameObject pointsText;
    private TextMeshProUGUI killsText;
    private TextMeshProUGUI carWalletCountText;
    private Image carWalletRadialUI;
    private TextMeshProUGUI tokensText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI currentCarNameText;
    private TextMeshProUGUI speedUpText;
    private TextMeshProUGUI missedChickenCountText;

    [Header("UI Popups")]
    [SerializeField] public GameObject positivePoints;
    [SerializeField] public GameObject negativePoints;
    [SerializeField] public GameObject positiveTokens;
    [SerializeField] public GameObject negativeTokens;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
        carWallet = FindObjectOfType<CarWallet>();
        speedUptextAnimator = speedUpText.GetComponent<Animator>();

        canvas = GameObject.Find(canvas_Name);
        pointsText = GameObject.Find(pointsText_Name);
        carWalletCountText = GameObject.Find(carWalletCount_Name).GetComponent<TextMeshProUGUI>();
        carWalletRadialUI = GameObject.Find(carWalletRadialUI_Name).GetComponent<Image>();
        tokensText = GameObject.Find(tokensText_Name).GetComponent<TextMeshProUGUI>();
        timeText = GameObject.Find(timeText_Name).GetComponent<TextMeshProUGUI>();
        currentCarNameText = GameObject.Find(currentCarText_Name).GetComponent<TextMeshProUGUI>();
        speedUpText = GameObject.Find(speedUpText_Name).GetComponent<TextMeshProUGUI>();
        missedChickenCountText = GameObject.Find(missedChickenCount_Name).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        pointsText.GetComponent<TextMeshProUGUI>().text = gameManager.playerScore.ToString("0000");
    }

    private void Update()
    {
        killsText.text = gameManager.killCount.ToString("000");
        tokensText.text = gameManager.tokens.ToString("000");
        timeText.text = gameManager.time.ToString("0");
        missedChickenCountText.text = gameManager.missedChickenLives.ToString("000");
        currentCarNameText.text = vehicleSpawner.currentActiveCar.carName;

        UpdateCarWalletUI(carWallet.timeUntilRefill, carWallet.refillDelaySeconds);
    }

    private void FixedUpdate()
    {
        if (scoreForText < gameManager.playerScore)
        {
            scoreForText += scoreMoverPositive;
        }
        else if (scoreForText > gameManager.playerScore)
        {
            scoreForText -= scoreMoverNegative;
        }
        pointsText.GetComponent<TextMeshProUGUI>().text = scoreForText.ToString("0000");
    }

    private void UpdateCarWalletUI(float timeRemaining, float maxCooldownTime)
    {
        carWalletCountText.text = carWallet.carCount.ToString("00");
        carWalletRadialUI.fillAmount = 1 - (timeRemaining / maxCooldownTime);
    }

    public void ShowSpeedUpText(string text = "Speed Up")
    {
        speedUpText.text = text;
        speedUpText.gameObject.SetActive(true);
        Invoke(nameof(FadeOut), speedUpTextDuration);
    }

    private void FadeOut()
    {
        speedUptextAnimator.SetTrigger(speedUpTextFadeOutTrigger);
        Invoke(nameof(DeactivateSpeedUpText), speedUpTextDeactivationDelay);
    }

    private void DeactivateSpeedUpText()
    {
        speedUpText.gameObject.SetActive(false);
    }

    public void ScoreUI(int points, bool ispositive)
    {
        Vector3 spawnLocation = new Vector3(0, 0, 0);
        if (ispositive)
        {
            GameObject score = Instantiate(positivePoints, spawnLocation, Quaternion.identity, canvas.transform);
            score.GetComponent<RectTransform>().localPosition = new Vector3(125f, 350f, 0);
            score.GetComponent<TextMeshProUGUI>().text = "+" + points.ToString();
            scoreMoverPositive = points / 50;
        }
        else
        {
            GameObject score = Instantiate(negativePoints, spawnLocation, Quaternion.identity, canvas.transform);
            score.GetComponent<RectTransform>().localPosition = new Vector3(-125f, 350f, 0);
            score.GetComponent<TextMeshProUGUI>().text = "-" + points.ToString();
            scoreMoverNegative = points / 50;
        }

    }

    public void TokenUI(int tokenAmount, bool ispositive)
    {
        Vector3 spawnLocation = new Vector3(0, 0, 0);
        if (ispositive)
        {
            GameObject token = Instantiate(positivePoints, spawnLocation, Quaternion.identity, canvas.transform);
            token.GetComponent<RectTransform>().anchoredPosition = new Vector2(-40f, 40f);
            token.GetComponent<TextMeshProUGUI>().text = "+" + tokenAmount.ToString();
        }
        else
        {
            GameObject token = Instantiate(negativePoints, spawnLocation, Quaternion.identity, canvas.transform);
            token.GetComponent<RectTransform>().localPosition = spawnLocation;
            token.GetComponent<TextMeshProUGUI>().text = "-" + tokenAmount.ToString();
        }
    }
}
