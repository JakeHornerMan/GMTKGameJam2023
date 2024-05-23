using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CarButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public TextMeshProUGUI tokenPriceText;
    [SerializeField] public TextMeshProUGUI carWeightText; [SerializeField] private Image correspCarIcon;

    [Header("Car Values")]
    [SerializeField] public Car correspondingCar;

    [Header("Colors")]
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;

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
    }

    private void OnDisable()
    {
        GameManager.OnTokensUpdated -= SetPriceColor;
    }
    private void Start()
    {
        if (correspondingCar)
        {
            tokenPriceText.text = "$" + correspondingCar.carPrice.ToString("0");
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
            tokenPriceText.color = enoughMoney ? positiveColor : negativeColor;
        }
    }

    public void SelectCorrespondingCar()
    {
        if (gameManager || tutorialManager)
            vehicleSpawner.SelectCar(this.correspondingCar);
    }
}
