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
    [SerializeField] private Image buttonOutline;

    [Header("Car Values")]
    [SerializeField] public Car correspondingCar;

    [Header("Colors")]
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;
    [SerializeField] private Color positiveColorOutline;
    [SerializeField] private Color negativeColorOutline;
    [SerializeField] private Color standardCarColorOutline;

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
            tokenPriceText.text = "$" + correspondingCar.carPrice.ToString("0");
            carWeightText.text = correspondingCar.carHealth.ToString("0");
            correspCarIcon.sprite = correspondingCar.GetComponent<ObjectInfo>().objectIcon;

            SetPriceColor();
            SetPriceColor();
        }
    }

    private void SetPriceColor()
    {
        if(gameManager){
            bool enoughMoney = gameManager.tokens >= correspondingCar.carPrice;
            tokenPriceText.color = enoughMoney ? positiveColor : negativeColor;

            buttonOutline.color = enoughMoney ? positiveColorOutline: negativeColorOutline;
            if(correspondingCar.name == "Standard Car"){
                buttonOutline.color = standardCarColorOutline;
            }
        }
        if(tutorialManager){
            bool enoughMoney = tutorialManager.tokens >= correspondingCar.carPrice;
            tokenPriceText.color = enoughMoney ? positiveColor : negativeColor;

            buttonOutline.color = enoughMoney ? positiveColorOutline: negativeColorOutline;
            if(correspondingCar.name == "Standard Car"){
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
