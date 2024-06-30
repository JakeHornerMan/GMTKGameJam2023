using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.AI;

public class CarButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public TextMeshProUGUI tokenPriceText;
    [SerializeField] private Image tokenIcon;
    [SerializeField] public TextMeshProUGUI carWeightText;
    [SerializeField] private Image correspCarIcon;
    [SerializeField] private Image buttonOutline;
    [SerializeField] private Image blueprintBG;

    [Header("Car Values")]
    [SerializeField] public Car correspondingCar;

    [Header("Colors")]
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;
    [SerializeField] private Sprite iconPositiveSprite;
    [SerializeField] private Sprite iconNegativeSprite;
    [SerializeField] private Sprite backgroundPositiveSprite;
    [SerializeField] private Sprite backgroundNegativeSprite;
    [SerializeField] private Color positiveColorOutline;
    [SerializeField] private Color negativeColorOutline;
    [SerializeField] private Color standardCarColorOutline;

    [Header("Coloration Settings")]
    [SerializeField] private bool enableBackgroundColoration = false;
    [SerializeField] private bool enableIconColoration = true;
    [SerializeField] private bool enableOutlineColoration = true;
    [SerializeField] private bool enableTextColoration = true;

    private VehicleSpawner vehicleSpawner;
    private GameManager gameManager;
    private TutorialManager tutorialManager;

    private void Awake()
    {
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
        gameManager = FindObjectOfType<GameManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void OnEnable()
    {
        GameManager.OnTokensUpdated += SetPriceColor;
        TutorialManager.OnTokensUpdated += SetPriceColor;
    }

    private void OnDisable()
    {
        GameManager.OnTokensUpdated -= SetPriceColor;
        TutorialManager.OnTokensUpdated -= SetPriceColor;
    }
    private void Start()
    {
        if (correspondingCar)
        {
            tokenPriceText.text = correspondingCar.carPrice.ToString("0");
            carWeightText.text = correspondingCar.carHealth.ToString("0");
            correspCarIcon.sprite = correspondingCar.GetComponent<ObjectInfo>().objectIcon;

            SetPriceColor();
        }
    }

    private void SetPriceColor()
    {
        if (gameManager)
        {
            bool enoughMoney = gameManager.tokens >= correspondingCar.carPrice;

            if (enableTextColoration)
                tokenPriceText.color = enoughMoney ? positiveColor : negativeColor;
            if (enableIconColoration)
                tokenIcon.sprite = enoughMoney ? iconPositiveSprite : iconNegativeSprite; // Color lightning icon
            if (enableBackgroundColoration)
                blueprintBG.sprite = enoughMoney ? backgroundPositiveSprite : backgroundNegativeSprite; // Color background BG
            if (enableOutlineColoration)
                buttonOutline.color = enoughMoney ? positiveColorOutline : negativeColorOutline;

            if (correspondingCar.name == "Standard Car")
            {
                buttonOutline.color = standardCarColorOutline;
            }
        }
        if (tutorialManager)
        {
            bool enoughMoney = tutorialManager.tokens >= correspondingCar.carPrice;
            tokenPriceText.color = enoughMoney ? positiveColor : negativeColor;

            buttonOutline.color = enoughMoney ? positiveColorOutline : negativeColorOutline;
            if (correspondingCar.name == "Standard Car")
            {
                buttonOutline.color = standardCarColorOutline;
            }
        }
    }

    public void SelectCorrespondingCar()
    {
        if (gameManager || tutorialManager)
            vehicleSpawner.SelectCar(this.correspondingCar);
    }
}
