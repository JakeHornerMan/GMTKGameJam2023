using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI carWalletCountText;
    [SerializeField] private Image carWalletRadialUI;
    [SerializeField] private TextMeshProUGUI tokensText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI currentCarNameText;
    [SerializeField] private TextMeshProUGUI speedUpText;
    [SerializeField] private TextMeshProUGUI missedChickenCountText;
    [SerializeField] private GameObject canvas;

    [Header("Animation")]
    [SerializeField] private string speedUpTextFadeOutTrigger = "FadeOut";

    [Header("Timing")]
    [SerializeField] private float speedUpTextDuration = 1.3f;
    [SerializeField] private float speedUpTextDeactivationDelay = 2;

    private GameManager gameManager;
    private VehicleSpawner vehicleSpawner;
    private CarWallet carWallet;
    private Animator speedUptextAnimator;

    [Header("Points Popups")]
    [SerializeField] public GameObject positivePoints;
    [SerializeField] public GameObject negativePoints;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
        carWallet = FindObjectOfType<CarWallet>();
        speedUptextAnimator = speedUpText.GetComponent<Animator>();
        canvas = GameObject.Find("Canvas");
    }

    private void Update()
    {
        pointsText.text = gameManager.playerScore.ToString("0000");
        killsText.text = gameManager.killCount.ToString("000");
        tokensText.text = gameManager.tokens.ToString("000");
        timeText.text = gameManager.time.ToString("0");
        missedChickenCountText.text = gameManager.safelyCrossedChickens.ToString("000");
        currentCarNameText.text = vehicleSpawner.currentActiveCar.carName;

        UpdateCarWalletUI(carWallet.timeUntilRefill, carWallet.refillDelaySeconds);
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

    public void ScoreUI(int points, bool ispositive){
        Vector3 spawnLocation = new Vector3(0, 0, 0);
        if(ispositive){
            GameObject score = GameObject.Instantiate(positivePoints, spawnLocation, Quaternion.identity, canvas.transform);
            score.GetComponent<RectTransform>().localPosition = new Vector3(125f, 350f, 0);
            score.GetComponent<TextMeshProUGUI>().text = "+"+points.ToString();
        }
        else{
            GameObject score = GameObject.Instantiate(negativePoints, spawnLocation, Quaternion.identity, canvas.transform);
            score.GetComponent<RectTransform>().localPosition = new Vector3(-125f, 350f, 0);
            score.GetComponent<TextMeshProUGUI>().text = "-"+points.ToString();
        }
        
    }
}
