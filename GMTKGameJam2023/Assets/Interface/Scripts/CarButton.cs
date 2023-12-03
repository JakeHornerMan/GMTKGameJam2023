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
    [SerializeField] public TextMeshProUGUI carWeightText;
    [SerializeField] private Image dollarIconImg;
    [SerializeField] private Image correspCarIcon;

    [Header("Car Values")]
    [SerializeField] public Car correspondingCar;

    [Header("Colors")]
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;

    private VehicleSpawner vehicleSpawner;
    private GameManager gameManager;

    private void Awake()
    {
        vehicleSpawner = FindObjectOfType<VehicleSpawner>();
        gameManager = FindObjectOfType<GameManager>(); 
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
        tokenPriceText.text = correspondingCar.carPrice.ToString("0");
        carWeightText.text = correspondingCar.carHealth.ToString("0");
        correspCarIcon.sprite = correspondingCar.GetComponent<ObjectInfo>().objectIcon;

        SetPriceColor();
    }

    private void SetPriceColor()
    {
        bool enoughMoney = gameManager.tokens >= correspondingCar.carPrice;
        tokenPriceText.color = dollarIconImg.color = enoughMoney ? positiveColor : negativeColor;
    }

    public void SelectCorrespondingCar()
    {
        vehicleSpawner.SelectCar(this);
    }
}
