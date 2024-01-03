using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pointsText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI carWalletCountText;
    [SerializeField] private Image carWalletIcon;
    [SerializeField] private Image carWalletRadialUI;
    [SerializeField] private Image ultimateRadialUI;
    [SerializeField] private GameObject[] carWalletNodes;
    [SerializeField] private GameObject[] carWalletNodeContainers;
    [SerializeField] private TextMeshProUGUI tokensText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI currentCarNameText;
    [SerializeField] private TextMeshProUGUI speedUpText;
    [SerializeField] private TextMeshProUGUI missedChickenCountText;
    [SerializeField] private GameObject canvas;

    [HideInInspector] private GameObject ultimateButton;

    [Header("Animation")]
    [SerializeField] private string speedUpTextFadeOutTrigger = "FadeOut";

    [Header("Timing")]
    [SerializeField] private float speedUpTextDuration = 1.3f;
    [SerializeField] private float speedUpTextDeactivationDelay = 2;

    private int scoreForText;
    private int scoreMoverPositive;
    private int scoreMoverNegative;

    private GameManager gameManager;
    private VehicleSpawner vehicleSpawner;
    private CarWallet carWallet;
    private UltimateManager ultimate;
    private Animator speedUptextAnimator;

    [Header("UI Popups")]
    [SerializeField] public GameObject positivePoints;
    [SerializeField] public GameObject negativePoints;
    [SerializeField] public GameObject positiveTokens;
    [SerializeField] public GameObject negativeTokens;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        ultimateButton = GameObject.Find("UltimateBtn");
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
        carWallet = FindObjectOfType<CarWallet>();
        ultimate = FindObjectOfType<UltimateManager>();
        speedUptextAnimator = speedUpText.GetComponent<Animator>();
        pointsText.GetComponent<TextMeshProUGUI>().text = gameManager.playerScore.ToString("0000");
    }

    private void Start()
    {
        if(ultimateButton != null){
            if(gameManager.ultimateInLevel == null){
                ultimateButton.SetActive(false);
            }
            else{
                ultimateButton.SetActive(true);
            }
        }

        for (int i = 0; i < carWalletNodeContainers.Length; i++){
            if(i < carWallet.walletLimit){
                carWalletNodeContainers[i].SetActive(true);
            }
            else{
                carWalletNodeContainers[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        killsText.text = gameManager.killCount.ToString("000");
        tokensText.text = gameManager.tokens.ToString("000");
        timeText.text = gameManager.time.ToString("0");
        missedChickenCountText.text = gameManager.missedChickenLives.ToString("000");
        // currentCarNameText.text = vehicleSpawner.currentActiveCar.GetComponent<ObjectInfo>().objectName;
        carWalletIcon.sprite = vehicleSpawner.currentActiveCar.GetComponent<ObjectInfo>().objectIcon;

        UpdateCarWalletUI(carWallet.timeUntilRefill, carWallet.refillDelaySeconds);
        UpdateUltimateRadial(ultimate.timeUntilRefill, ultimate.refillDelaySeconds);
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
        for (int i = 0; i < carWalletNodes.Length; i++){
            if(i < carWallet.carCount){
                carWalletNodes[i].SetActive(true);
            }
            else{
                carWalletNodes[i].SetActive(false);
            }
        }
        
        carWalletCountText.text = carWallet.carCount.ToString("00");
        carWalletRadialUI.fillAmount = 1 - (timeRemaining / maxCooldownTime);
    }

    private void UpdateUltimateRadial(float timeRemaining, float maxCooldownTime)
    {
        ultimateRadialUI.fillAmount = 1 - (timeRemaining / maxCooldownTime);
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
